using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class Monster_BattleLogic : MonoBehaviour
{
    [Header("内部组件引用")]
    [SerializeField] private MonsterStatus monsterStatus;
    [SerializeField] private Animator animator;
    [SerializeField] private MonsterTimer_AttackInterval attackIntervalTimer;// 攻击间隔定时器组件
    [SerializeField] private AutoLockSystem autoLockSystem; // 自动锁定系统组件

    [Header("缓存攻击点的Transform引用")]
    [SerializeField] private Transform attackPoint_Horizontal;
    [SerializeField] private Transform attackPoint_Up;
    [SerializeField] private Transform attackPoint_Down;

    [Header("箭矢预制体引用")]
    [SerializeField] private GameObject arrowPrefab;

    private IObjectPool<ArrowMovement02> arrowPool;// 箭矢对象池
    private Transform playerTransform;
    private Vector2 attackDirection;


    void Awake()
    {
        autoLockSystem.onLockTargetChange += UpdateTarget;
    }

    void Start()
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
    }

    private void UpdateTarget(Transform target)
    {
        playerTransform = target;
    }

    /// <summary>
    /// 检测攻击范围内是否存在玩家，并根据怪物职业切换到攻击或射击状态。
    /// </summary>
    private void Judge_MonsterCurrentState_AttackOrShoot()
    {
        if (monsterStatus.CanAttack == false || playerTransform == null) return;// 如果不能攻击或者没有目标，则直接返回

        attackDirection = (playerTransform.position - transform.position).normalized;// 计算攻击方向

        // 如果找到了玩家，并且玩家在攻击范围内，则根据职业切换状态
        if (Vector2.Distance(transform.position, playerTransform.position) <= monsterStatus.monsterProperty.attackRange)
        {
            // 怪物职业是战士时，切换到Attack状态
            if (monsterStatus.monsterProperty.profession == MonsterProperty.ProfessionType.Warrior)
            {
                monsterStatus.CurrentAttackType = DetermineAttackType();// 确定Attack类型
                monsterStatus.CurrentState = MonsterStatus.MonsterState.Attack;
            }

            // 职业为弓箭手时，切换到Shoot状态
            else if (monsterStatus.monsterProperty.profession == MonsterProperty.ProfessionType.Archer)
            {
                monsterStatus.CurrentState = MonsterStatus.MonsterState.Shoot;
            }
        }
    }

    /// <summary>
    /// 检查是否应该将怪物状态切换为跑动或待机。
    /// </summary>
    private void Judge_MonsterCurrentState_RunOrIdle()
    {
        if (playerTransform == null) return;// 如果没有玩家，则直接返回


        // 计算玩家与怪物之间的距离
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 玩家在锁定范围之外，且怪物当前不是Idle状态 -> 切换到Idle
        if (distanceToPlayer > autoLockSystem.lockRange)
        {
            monsterStatus.CurrentState = MonsterStatus.MonsterState.Idle;
        }
        // 玩家在侦测范围之内，但在攻击范围之外 -> 切换到Run
        else if (distanceToPlayer > monsterStatus.monsterProperty.attackRange || (distanceToPlayer <= monsterStatus.monsterProperty.attackRange && monsterStatus.CanAttack == false))
        {
            monsterStatus.CurrentState = MonsterStatus.MonsterState.Run;
        }
    }


    /// <summary>
    /// 根据玩家位置和怪物位置确定攻击类型
    /// </summary>
    /// <returns>返回确定的攻击类型</returns>
    public MonsterStatus.AttackType DetermineAttackType()
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
            return MonsterStatus.AttackType.Attack_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return MonsterStatus.AttackType.Attack_Up;
        }
        else
        {
            return MonsterStatus.AttackType.Attack_Down;
        }
    }

    // 根据当前状态播放对应动画   
    public void JudgeMonsterState()
    {
        if (monsterStatus.CanAttack == true) Judge_MonsterCurrentState_AttackOrShoot();// 优先检查攻击/射击状态
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
        switch (monsterStatus.CurrentAttackType)
        {
            case MonsterStatus.AttackType.Attack_Horizontal:
                animator.Play("Attack_Horizontal");
                break;
            case MonsterStatus.AttackType.Attack_Up:
                animator.Play("Attack_Up");
                break;
            case MonsterStatus.AttackType.Attack_Down:
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



    //激活相应的攻击点
    private void Active_AttackPoint()
    {
        switch (monsterStatus.CurrentAttackType)
        {
            case MonsterStatus.AttackType.Attack_Horizontal:
                // 水平攻击逻辑
                attackPoint_Horizontal.gameObject.SetActive(true);
                break;
            case MonsterStatus.AttackType.Attack_Up:
                // 向上攻击逻辑
                attackPoint_Up.gameObject.SetActive(true);
                break;
            case MonsterStatus.AttackType.Attack_Down:
                // 向下攻击逻辑
                attackPoint_Down.gameObject.SetActive(true);
                break;
        }
    }

    //关闭所有攻击点
    private void Deactive_AttackPoint()
    {
        attackPoint_Horizontal.gameObject.SetActive(false);
        attackPoint_Up.gameObject.SetActive(false);
        attackPoint_Down.gameObject.SetActive(false);
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
        attackPoint_Horizontal.gameObject.SetActive(false);
        attackPoint_Up.gameObject.SetActive(false);
        attackPoint_Down.gameObject.SetActive(false);

        monsterStatus.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        monsterStatus.CanChangeAttackType = true; // 同时解锁攻击类型切换
        monsterStatus.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击

        attackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }


    /// <summary>
    /// AnimationEvent:Shoot动画结束时调用
    /// </summary>
    public void ShootEnd()
    {
        OutShootArrow(); // 生成箭矢并发射

        monsterStatus.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        monsterStatus.CanChangeAttackType = true; // 同时解锁攻击类型切换
        monsterStatus.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击


        attackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }
    /// <summary>
    /// 怪物受到伤害时调用，减少生命值
    /// </summary>
    /// <param name="amount">伤害值</param>
    public void TakeDamage(float amount)
    {
        monsterStatus.CurrentHealth -= amount;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }


    //**************************************箭矢对象池方法*************************************************************
    // 创建箭矢实例的方法（由对象池调用）
    private ArrowMovement02 CreateArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return null;
        }

        // 生成箭矢在世界坐标系中，位置相对于monster
        Vector3 spawnPosition = transform.position + new Vector3(-0.011f, -0.118f, -1f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // 设置箭矢的初始飞行方向，以及发射箭矢的玩家对象引用
        ArrowMovement02 arrowMovement02 = arrow.GetComponent<ArrowMovement02>();
        arrowMovement02.Pool = arrowPool; // 设置箭矢的对象池引用
        if (arrowMovement02 != null)
        {
            arrowMovement02.Direction = attackDirection;
            arrowMovement02.Damage = monsterStatus.monsterProperty.damage;
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
        arrow.transform.position = transform.position + new Vector3(-0.011f, -0.118f, -1f); // 每次发射时重置箭矢位置
        arrow.transform.rotation = Quaternion.identity; // 每次发射时重置箭矢旋转角度
        arrow.transform.right = attackDirection; // 每次发射时调整箭矢朝向攻击方向
        arrow.Direction = attackDirection; // 每次发射时重置箭矢方向 
        arrow.Damage = monsterStatus.monsterProperty.damage; // 每次发射时重置箭矢伤害值（如果玩家的伤害属性发生了变化）
        arrow.gameObject.SetActive(true);
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
    //射出箭矢
    private void OutShootArrow()
    {
        arrowPool.Get();
    }
}