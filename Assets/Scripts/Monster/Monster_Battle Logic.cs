using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

[RequireComponent(typeof(Monster_PropertyManager), typeof(Animator))]
public class Monster_BattleLogic : MonoBehaviour
{
    private Monster_PropertyManager monster;
    private Animator animator;
    public Transform playerTransform;

    private GameObject ArrowPrefab;// 箭矢预制体引用

    private Vector2 attackDirection;

    // 缓存攻击点的Transform引用
    private Transform AttackPoint_Horizontal;
    private Transform AttackPoint_Up ;
    private Transform AttackPoint_Down ;
    private MonsterTimer_AttackInterval AttackIntervalTimer;// 攻击间隔定时器组件
    private IObjectPool<ArrowMovement02> arrowPool;// 箭矢对象池


    void Awake()
    {
        // 创建箭矢对象池
        arrowPool = new ObjectPool<ArrowMovement02>(
        createFunc: CreateArrow,           // 创建新对象的方法
        actionOnGet: OnGetArrow,           // 从池中取出时的回调
        actionOnRelease: OnReleaseArrow,   // 归还时的回调
        actionOnDestroy: OnDestroyArrow,   // 销毁时的回调（池满时）
        collectionCheck: false,
        defaultCapacity: 30,
        maxSize: 50
        );


        monster = GetComponent<Monster_PropertyManager>();
        animator = GetComponent<Animator>();
        ArrowPrefab = Resources.Load<GameObject>("Prefabs/ArrowPrefab02");


        // 查找子对象AttackPoint并缓存攻击点的Transform引用
        AttackPoint_Horizontal = transform.Find("AttackPoint_Horizontal");
        AttackPoint_Up = transform.Find("AttackPoint_Up");
        AttackPoint_Down = transform.Find("AttackPoint_Down");

        AttackIntervalTimer = GetComponent<MonsterTimer_AttackInterval>();// 缓存攻击间隔定时器组件
    }

    /// <summary>
    /// 检测攻击范围内是否存在玩家，并根据怪物职业切换到攻击或射击状态。
    /// </summary>
    private void Judge_MonsterCurrentState_AttackOrShoot()
    {
        if(monster.CanAttack == false)return;// 如果不能攻击，则直接返回

        // 使用 OverlapCircleAll 检测范围内的所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monster.AttackRange); 
        
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                { 
                    closestDistance = distance;
                    closestPlayer = hit.transform;
                    attackDirection = (closestPlayer.position - transform.position).normalized;// 计算攻击方向
                }
            }
        }
        playerTransform = closestPlayer;

        // 如果找到了玩家，并且玩家在攻击范围内，则根据职业切换状态
        if (closestPlayer != null &&  closestDistance <= monster.AttackRange )
        {
            // 怪物职业是战士时，切换到Attack状态
            if (monster.Profession == Monster_PropertyManager.ProfessionType.Warrior)
            {
                monster.CurrentAttackType = DetermineAttackType();// 确定Attack类型
                monster.CurrentState = Monster_PropertyManager.MonsterState.Attack;
            }

            // 职业为弓箭手时，切换到Shoot状态
            else if (monster.Profession == Monster_PropertyManager.ProfessionType.Archer)
            {
                monster.CurrentState = Monster_PropertyManager.MonsterState.Shoot;
            }          
        }
    }

    /// <summary>
    /// 检查是否应该将怪物状态切换为跑动或待机。
    /// </summary>
    private void Judge_MonsterCurrentState_RunOrIdle()
    {
        // 使用 OverlapCircleAll 检测范围内的所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monster.DetectionRange); 
        
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                { 
                    closestDistance = distance;
                    closestPlayer = hit.transform;
                }
            }
        }
        playerTransform = closestPlayer;
        if(playerTransform == null)return;// 如果没有玩家，则直接返回


        // 计算玩家与怪物之间的距离
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 玩家在侦测范围之外，且怪物当前不是Idle状态 -> 切换到Idle
        if (distanceToPlayer > monster.DetectionRange)
        {
            monster.CurrentState = Monster_PropertyManager.MonsterState.Idle;
        }
        // 玩家在侦测范围之内，但在攻击范围之外 -> 切换到Run
        else if (distanceToPlayer > monster.AttackRange || (distanceToPlayer <= monster.AttackRange && monster.CanAttack == false))
        {
            monster.CurrentState = Monster_PropertyManager.MonsterState.Run;
        }
    }


    /// <summary>
    /// 根据玩家位置和怪物位置确定攻击类型
    /// </summary>
    /// <returns>返回确定的攻击类型</returns>
    public Monster_PropertyManager.AttackType DetermineAttackType()
    {
        // 根据玩家位置和怪物位置确定攻击方向
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            // 水平攻击前，根据玩家方向调整怪物朝向
            Vector3 currentScale = transform.localScale;
            if (attackDirection.x < 0)
            {
                // 玩家在左边，朝向左
                currentScale.x = -Mathf.Abs(currentScale.x);
            }
            else
            {
                // 玩家在右边，朝向右
                currentScale.x = Mathf.Abs(currentScale.x);
            }
            transform.localScale = currentScale;
            return Monster_PropertyManager.AttackType.Attack_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return Monster_PropertyManager.AttackType.Attack_Up;
        }
        else
        {
            return Monster_PropertyManager.AttackType.Attack_Down;
        }
    }

    // 根据当前状态播放对应动画   
        public void JudgeMonsterState()
    {
        if(monster.CanAttack == true)Judge_MonsterCurrentState_AttackOrShoot();// 优先检查攻击/射击状态
        Judge_MonsterCurrentState_RunOrIdle();    
        
    }


    public void PlayAttackAnimation()
    {
        
                if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
                {
                    // 水平攻击前，根据玩家方向调整怪物朝向
                    Vector3 currentScale = transform.localScale;
                    if (attackDirection.x < 0)
                    {
                        // 玩家在左边，朝向左
                        currentScale.x = -Mathf.Abs(currentScale.x);
                    }
                    else
                    {
                        // 玩家在右边，朝向右
                        currentScale.x = Mathf.Abs(currentScale.x);
                    }
                    transform.localScale = currentScale;
                }


                // 根据攻击方向选择攻击动画
                switch (monster.CurrentAttackType)
                {
                    case Monster_PropertyManager.AttackType.Attack_Horizontal:
                        animator.Play("Attack_Horizontal");
                        break;
                    case Monster_PropertyManager.AttackType.Attack_Up:
                        animator.Play("Attack_Up");
                        break;
                    case Monster_PropertyManager.AttackType.Attack_Down:
                        animator.Play("Attack_Down");
                        break;
                }
                
        }


    public void PlayShootAnimation()
    {

                if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
                {
                    // 水平攻击前，根据玩家方向调整怪物朝向
                    Vector3 currentScale = transform.localScale;
                    if (attackDirection.x < 0)
                    {
                        // 玩家在左边，朝向左
                        currentScale.x = -Mathf.Abs(currentScale.x);
                    }
                    else
                    {
                        // 玩家在右边，朝向右
                        currentScale.x = Mathf.Abs(currentScale.x);
                    }
                    transform.localScale = currentScale;
                }
                animator.Play("Shoot");
    }

    public void PlayIdleAnimation()
    {
        animator.SetBool("isRun", false);
    }

    public void PlayRunAnimation()
    {
        animator.SetBool("isRun", true);
    }

    //AnimationEvent：射出箭矢
    private void OutShootArrow()
    {
        arrowPool.Get();
    }

    //激活相应的攻击点
    private void Active_AttackPoint()
    {
        switch (monster.CurrentAttackType)
        {
            case Monster_PropertyManager.AttackType.Attack_Horizontal:
                // 水平攻击逻辑
                AttackPoint_Horizontal.gameObject.SetActive(true);
                break;
            case Monster_PropertyManager.AttackType.Attack_Up:
                // 向上攻击逻辑
                AttackPoint_Up.gameObject.SetActive(true);
                break;
            case Monster_PropertyManager.AttackType.Attack_Down:
                // 向下攻击逻辑
                AttackPoint_Down.gameObject.SetActive(true);
                break;
        }
    }

    //关闭所有攻击点
    private void Deactive_AttackPoint()
    {
        AttackPoint_Horizontal.gameObject.SetActive(false);
        AttackPoint_Up.gameObject.SetActive(false);
        AttackPoint_Down.gameObject.SetActive(false);
    }

    /// <summary>
    /// AnimationEvent:Attack动画开始时调用
    /// </summary>
    public void AttackBegin()
    {
        Active_AttackPoint();// 激活当前攻击类型的攻击点
    }


    /// <summary>
    /// AnimationEvent:Attack动画结束时调用
    /// </summary>
    public void AttackEnd()
    {
        // 攻击结束时，关闭所有攻击点
        AttackPoint_Horizontal.gameObject.SetActive(false);
        AttackPoint_Up.gameObject.SetActive(false);
        AttackPoint_Down.gameObject.SetActive(false);

        monster.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        monster.CanChangeAttackType = true; // 同时解锁攻击类型切换
        monster.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击

        AttackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }


    /// <summary>
    /// AnimationEvent:Shoot动画结束时调用
    /// </summary>
    public void ShootEnd()
    {
        OutShootArrow(); // 生成箭矢并发射

        monster.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        monster.CanChangeAttackType = true; // 同时解锁攻击类型切换
        monster.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击


        AttackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }
    /// <summary>
    /// 怪物受到伤害时调用，减少生命值
     /// </summary>
    /// <param name="amount">伤害值</param>
    public void TakeDamage(float amount)
    {
        monster.CurrentHealth -= amount;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }


//**************************************箭矢对象池方法*************************************************************
    // 创建箭矢实例的方法（由对象池调用）
    private ArrowMovement02 CreateArrow()
    {
        if (ArrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return null;
        }
      
        // 生成箭矢在世界坐标系中，位置相对于monster
        Vector3 spawnPosition = transform.position + new Vector3(-0.011f, -0.118f, -1f);
        GameObject arrow = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);

        // 设置箭矢的初始飞行方向，以及发射箭矢的玩家对象引用
        ArrowMovement02 arrowMovement02 = arrow.GetComponent<ArrowMovement02>();
        arrowMovement02.Pool = arrowPool; // 设置箭矢的对象池引用
        if (arrowMovement02 != null)
        {
            arrowMovement02.Direction = attackDirection;
            arrowMovement02.Damage = monster.Damage; 
        }
        else
        {
            Debug.LogError("箭矢预制体缺少 ArrowMovement 组件，无法设置飞行方向和伤害值。");
        }
        // 设置箭矢朝向攻击方向（箭矢默认朝右）
        if (attackDirection != Vector2.zero)
        {
            // 将箭头的右端朝向攻击方向（箭头默认朝右）
            arrow.transform.right = attackDirection;
        }
        return arrowMovement02;
    }

    private void OnGetArrow(ArrowMovement02 arrow)
    {
        arrow.gameObject.SetActive(true);
        arrow.transform.position = transform.position + new Vector3(-0.011f, -0.118f, -1f); // 每次发射时重置箭矢位置
        arrow.transform.rotation = Quaternion.identity; // 每次发射时重置箭矢旋转角度
        arrow.transform.right = attackDirection; // 每次发射时调整箭矢朝向攻击方向
        arrow.Direction = attackDirection; // 每次发射时重置箭矢方向 
        arrow.Damage = monster.Damage; // 每次发射时重置箭矢伤害值（如果玩家的伤害属性发生了变化）
        
    }


    private void OnReleaseArrow(ArrowMovement02 arrow)
    {
        arrow.gameObject.SetActive(false);
    }


    private void OnDestroyArrow(ArrowMovement02 arrow)
    {
        Destroy(arrow.gameObject);
    }
//**************************************箭矢对象池方法*************************************************************

}