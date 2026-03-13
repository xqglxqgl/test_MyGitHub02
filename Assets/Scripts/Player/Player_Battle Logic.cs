using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class Player_BattleLogic : MonoBehaviour
{
    private Vector2 attackDirection;    // 当前目标方向，用于决定动画

    // 攻击点的Transform引用
    private Transform AttackPoint_Horizontal;
    private Transform AttackPoint_Up ;
    private Transform AttackPoint_Down ;

    private Player_PropertyManager player;
    private Animator Animator;
    private GameObject ArrowPrefab;     // 箭矢预制体引用
    private PlayerTimer_AttackInterval AttackIntervalTimer;// 攻击间隔定时器组件

    private IObjectPool<ArrowMovement> arrowPool;// 箭矢对象池

    void Awake()
    {
        // 查找子对象AttackPoint并缓存攻击点的Transform引用
        AttackPoint_Horizontal = transform.Find("AttackPoint_Horizontal");
        AttackPoint_Up = transform.Find("AttackPoint_Up");
        AttackPoint_Down = transform.Find("AttackPoint_Down");
        // 创建箭矢对象池
        arrowPool = new ObjectPool<ArrowMovement>(
        createFunc: CreateArrow,           // 创建新对象的方法
        actionOnGet: OnGetArrow,           // 从池中取出时的回调
        actionOnRelease: OnReleaseArrow,   // 归还时的回调
        actionOnDestroy: OnDestroyArrow,   // 销毁时的回调（池满时）
        collectionCheck: false,
        defaultCapacity: 30,
        maxSize: 50
        );
    }

    void Start()
    {

        // 在自身查找 Player_PropertyManager 组件
        player = GetComponent<Player_PropertyManager>();
        if (player == null)
        {
            Debug.LogError("Player_BattleLogic 无法找到 Player_PropertyManager 组件，请检查挂载位置。");
            enabled = false;
            return;
        }

        Animator = GetComponent<Animator>();
        if (Animator == null)
        {
            Debug.LogError("Player_BattleLogic 需要一个 Animator 组件来播放攻击动画。");
            enabled = false;
            return;
        }

        // 加载箭矢预制体
        ArrowPrefab = Resources.Load<GameObject>("Prefabs/ArrowPrefab");
        if (ArrowPrefab == null)
        {
            Debug.LogError("无法加载 ArrowPrefab 预制体，请确保它在 Resources/Prefabs 文件夹中。");
        }
        // 缓存攻击间隔定时器组件
        AttackIntervalTimer = GetComponent<PlayerTimer_AttackInterval>();
        if (AttackIntervalTimer == null)
        {
            Debug.LogError("Player_BattleLogic 无法找到 PlayerTimer_AttackInterval 组件，请检查挂载位置。");
            enabled = false;
            return;
        }

    }

    public void Judge_PlyaerCurrentState_AttackOrShoot()
    {
        if(player.CanAttack == false)return;// 如果不能攻击，则直接返回

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, player.AttackRange);// 查找所有在攻击范围内的敌人
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Monster"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    // 更新最近敌人和距离,锁定最近的敌人
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null )
        {
            attackDirection = (closestEnemy.position - transform.position).normalized;// 计算攻击方向并归一化

            // 根据敌人位置调整玩家水平朝向,使得玩家始终面向敌人
            if (attackDirection.x > 0)
                transform.localScale = new Vector3(1, 1, 1);  // 面向右
            else if (attackDirection.x < 0)
                transform.localScale = new Vector3(-1, 1, 1); // 面向左

            // 根据职业判断是进行攻击还是射击
                // 弓箤手职业：使用射击状态
            if (player.Profession == Player_PropertyManager.ProfessionType.Archer)
            {
                // 根据攻击方向判断进行射击的类型，并尝试切换到该射击类型
                player.CurrentShootType = DetermineShootType();

                // 切换到 Shoot 状态
                player.CurrentState = Player_PropertyManager.PlayerState.Shoot;
            }

                // 战士职业：使用攻击状态
            else if (player.Profession == Player_PropertyManager.ProfessionType.Warrior)
            {
                // 根据攻击方向判断进行进进行攻击的类型，并尝试切换到该攻击类型
                player.CurrentAttackType = DetermineAttackType();

                // 切换到 Attack 状态
                player.CurrentState = Player_PropertyManager.PlayerState.Attack;

            } 
        }
    }

    /// <summary>
    /// 根据攻击方向判断进行进进行攻击的类型
    /// </summary>
    private Player_PropertyManager.AttackType DetermineAttackType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            return Player_PropertyManager.AttackType.Attack_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return Player_PropertyManager.AttackType.Attack_Up;
        }
        else
        {
            return Player_PropertyManager.AttackType.Attack_Down;
        }
    }

    /// <summary>
    /// 根据射击方向判断进行射击的类型
    /// </summary>
    private Player_PropertyManager.ShootType DetermineShootType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {   
            return Player_PropertyManager.ShootType.Shoot_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_PropertyManager.ShootType.Shoot_Diagonal_Up : Player_PropertyManager.ShootType.Shoot_Up;
        }
        else
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_PropertyManager.ShootType.Shoot_Diagonal_Down : Player_PropertyManager.ShootType.Shoot_Down;
        }
    }

    public void PlayMeleeAttackAnimation()
    {

        switch (player.CurrentAttackType)
        {
            case Player_PropertyManager.AttackType.Attack_Horizontal:
                Animator.Play("Attack_Horizontal");
                break;
            case Player_PropertyManager.AttackType.Attack_Up:
                Animator.Play("Attack_Up");
                break;
            case Player_PropertyManager.AttackType.Attack_Down:
                Animator.Play("Attack_Down");
                break;
        }
    }

    public void PlayShootAnimation()
    {

        switch (player.CurrentShootType)
        {
            case Player_PropertyManager.ShootType.Shoot_Horizontal:
                Animator.Play("Shoot_Horizontal");
                break;
            case Player_PropertyManager.ShootType.Shoot_Up:
                Animator.Play("Shoot_Up");
                break;
            case Player_PropertyManager.ShootType.Shoot_Down:
                Animator.Play("Shoot_Down");
                break;
            case Player_PropertyManager.ShootType.Shoot_Diagonal_Up:
                Animator.Play("Shoot_Diagonal_Up");
                break;
            case Player_PropertyManager.ShootType.Shoot_Diagonal_Down:
                Animator.Play("Shoot_Diagonal_Down");
                break;
        }
    }

//**************************************箭矢对象池方法*************************************************************
    // 创建箭矢实例的方法（由对象池调用）
    private ArrowMovement CreateArrow()
    {
        if (ArrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return null;
        }
      
        // 生成箭矢在世界坐标系中，位置相对于Player
        Vector3 spawnPosition = transform.position + new Vector3(-0.011f, -0.118f, -1f);
        GameObject arrow = Instantiate(ArrowPrefab, spawnPosition, Quaternion.identity);

        // 设置箭矢的初始飞行方向，以及发射箭矢的玩家对象引用
        ArrowMovement arrowMovement = arrow.GetComponent<ArrowMovement>();
        arrowMovement.Pool = arrowPool; // 设置箭矢的对象池引用
        if (arrowMovement != null)
        {
            arrowMovement.Direction = attackDirection;
            arrowMovement.Damage = player.Damage; // 传入该发出箭矢的玩家对象引用，方便访问该玩家的属性
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
        return arrowMovement;
    }

    private void OnGetArrow(ArrowMovement arrow)
    {
        arrow.gameObject.SetActive(true);
        arrow.transform.position = transform.position + new Vector3(-0.011f, -0.118f, -1f); // 每次发射时重置箭矢位置
        arrow.transform.rotation = Quaternion.identity; // 每次发射时重置箭矢旋转角度
        arrow.transform.right = attackDirection; // 每次发射时调整箭矢朝向攻击方向
        arrow.Direction = attackDirection; // 每次发射时重置箭矢方向 
        arrow.Damage = player.Damage; // 每次发射时重置箭矢伤害值（如果玩家的伤害属性发生了变化）
        
    }


    private void OnReleaseArrow(ArrowMovement arrow)
    {
        arrow.gameObject.SetActive(false);
    }


    private void OnDestroyArrow(ArrowMovement arrow)
    {
        Destroy(arrow.gameObject);
    }
//**************************************箭矢对象池方法*************************************************************

    //用于实际生成并发射箭矢。
    private void OutShootArrow()
    {
        arrowPool.Get(); // 从对象池中获取一个箭矢实例并发射
    }

    //激活近战攻击点（根据当前攻击类型）
    private void Active_AttackPoint()
    {
        switch (player.CurrentAttackType)
        {
            case Player_PropertyManager.AttackType.Attack_Horizontal:
                // 水平攻击逻辑
                AttackPoint_Horizontal.gameObject.SetActive(true);
                break;
            case Player_PropertyManager.AttackType.Attack_Up:
                // 向上攻击逻辑
                AttackPoint_Up.gameObject.SetActive(true);
                break;
            case Player_PropertyManager.AttackType.Attack_Down:
                // 向下攻击逻辑
                AttackPoint_Down.gameObject.SetActive(true);
                break;
        }
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

        player.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        player.CanChangeAttackType = true; // 同时解锁攻击类型切换
        player.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击

        AttackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }


    /// <summary>
    /// AnimationEvent:Shoot动画结束时调用
    /// </summary>
    public void ShootEnd()
    {
        OutShootArrow(); // 生成箭矢并发射

        player.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        player.CanChangeAttackType = true; // 同时解锁攻击类型切换
        player.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击


        AttackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }

    /// <summary>
    /// Player受到伤害时调用，减少生命值
    /// </summary>
    /// <param name="amount">伤害值</param>
    public void TakeDamage(float amount)
    {
        player.CurrentHealth -= amount;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }


}