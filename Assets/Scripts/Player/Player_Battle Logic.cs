using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class Player_BattleLogic : MonoBehaviour
{

    [Header("外部组件引用")]
    [SerializeField] private Transform attackPoint_Horizontal;
    [SerializeField] private Transform attackPoint_Up ;
    [SerializeField] private Transform attackPoint_Down ;
    [SerializeField] private GameObject arrowPrefab;     // 箭矢预制体引用
   


    [Header("自身组件引用")]
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerTimer_AttackInterval attackIntervalTimer;// 攻击间隔定时器组件
    [SerializeField] private AutoLockSystem_WithCursor autoLockSystem;// 自动锁定系统组件

    private PlayerProperty playerProperty;// 玩家基础属性
    private IObjectPool<ArrowMovement> arrowPool;// 箭矢对象池
    private Transform currentTarget; // 当前目标敌人，用于锁定和攻击
    private Vector2 attackDirection;    // 当前目标方向，用于决定动画

    void Awake()
    {
        playerProperty = playerStatus.playerProperty;
    }

    void Start()
    {
        GameManager.Instance.onPlayerTakeDamage += OnTakeDamage;// 订阅玩家受到伤害事件
        autoLockSystem.onLockTargetChange += OnLockTargetChange;// 订阅自动锁定系统目标改变事件

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
    private void OnLockTargetChange(Transform newTarget)
    {
        currentTarget = newTarget;
    }


    public void Judge_PlyaerCurrentState_AttackOrShoot()
    {
        if(playerStatus.CurrentState == PlayerStatus.PlayerState.Attack)return;// 如果当前状态为攻击，则直接返回

        if (currentTarget != null )
        {
            attackDirection = (currentTarget.position - transform.position).normalized;// 计算攻击方向并归一化

            // 根据敌人位置调整玩家水平朝向,使得玩家始终面向敌人
            if (attackDirection.x > 0)
                transform.localScale = new Vector3(1, 1, 1);  // 面向右
            else if (attackDirection.x < 0)
                transform.localScale = new Vector3(-1, 1, 1); // 面向左

            float distance = Vector2.Distance(transform.position, currentTarget.position);// 计算玩家与敌人的距离
            if(distance <= playerProperty.attackRange)// 如果敌人在攻击范围内
            {
                // 根据职业判断是进行攻击还是射击
                    // 弓箤手职业：使用射击状态
                if (playerProperty.profession == PlayerProperty.ProfessionType.Archer)
                {
                    // 根据攻击方向判断进行射击的类型，并尝试切换到该射击类型
                    playerStatus.CurrentShootType = DetermineShootType();

                    // 切换到 Shoot 状态
                    playerStatus.CurrentState = PlayerStatus.PlayerState.Shoot;
                }

                    // 战士职业：使用攻击状态
                else if (playerProperty.profession == PlayerProperty.ProfessionType.Warrior)
                {
                    // 根据攻击方向判断进行进进行攻击的类型，并尝试切换到该攻击类型
                    playerStatus.CurrentAttackType = DetermineAttackType();

                    // 切换到 Attack 状态
                    playerStatus.CurrentState = PlayerStatus.PlayerState.Attack;

                } 
            }
        }
    }

    /// <summary>
    /// 根据攻击方向判断进行进进行攻击的类型
    /// </summary>
    private PlayerStatus.AttackType DetermineAttackType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            return PlayerStatus.AttackType.Attack_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return PlayerStatus.AttackType.Attack_Up;
        }
        else
        {
            return PlayerStatus.AttackType.Attack_Down;
        }
    }

    /// <summary>
    /// 根据射击方向判断进行射击的类型
    /// </summary>
    private PlayerStatus.ShootType DetermineShootType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {   
            return PlayerStatus.ShootType.Shoot_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? PlayerStatus.ShootType.Shoot_Diagonal_Up : PlayerStatus.ShootType.Shoot_Up;
        }
        else
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? PlayerStatus.ShootType.Shoot_Diagonal_Down : PlayerStatus.ShootType.Shoot_Down;
        }
    }

    public void PlayMeleeAttackAnimation()
    {

        switch (playerStatus.CurrentAttackType)
        {
            case PlayerStatus.AttackType.Attack_Horizontal:
                animator.Play("Attack_Horizontal");
                break;
            case PlayerStatus.AttackType.Attack_Up:
                animator.Play("Attack_Up");
                break;
            case PlayerStatus.AttackType.Attack_Down:
                animator.Play("Attack_Down");
                break;
        }
    }

    public void PlayShootAnimation()
    {

        switch (playerStatus.CurrentShootType)
        {
            case PlayerStatus.ShootType.Shoot_Horizontal:
                animator.Play("Shoot_Horizontal");
                break;
            case PlayerStatus.ShootType.Shoot_Up:
                animator.Play("Shoot_Up");
                break;
            case PlayerStatus.ShootType.Shoot_Down:
                animator.Play("Shoot_Down");
                break;
            case PlayerStatus.ShootType.Shoot_Diagonal_Up:
                animator.Play("Shoot_Diagonal_Up");
                break;
            case PlayerStatus.ShootType.Shoot_Diagonal_Down:
                animator.Play("Shoot_Diagonal_Down");
                break;
        }
    }

//**************************************箭矢对象池方法*************************************************************
    // 创建箭矢实例的方法（由对象池调用）
    private ArrowMovement CreateArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return null;
        }
      
        // 生成箭矢在世界坐标系中，位置相对于Player
        Vector3 spawnPosition = transform.position + new Vector3(-0.011f, -0.118f, -1f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // 设置箭矢的初始飞行方向，以及发射箭矢的玩家对象引用
        ArrowMovement arrowMovement = arrow.GetComponent<ArrowMovement>();
        arrowMovement.Pool = arrowPool; // 设置箭矢的对象池引用
        if (arrowMovement != null)
        {
            arrowMovement.Direction = attackDirection;
            arrowMovement.Damage = playerProperty.damage; // 传入该发出箭矢的玩家对象引用，方便访问该玩家的属性
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
        arrow.transform.position = transform.position + new Vector3(-0.011f, -0.118f, -1f); // 每次发射时重置箭矢位置
        arrow.transform.rotation = Quaternion.identity; // 每次发射时重置箭矢旋转角度
        arrow.transform.right = attackDirection; // 每次发射时调整箭矢朝向攻击方向
        arrow.Direction = attackDirection; // 每次发射时重置箭矢方向 
        arrow.Damage = playerProperty.damage; // 每次发射时重置箭矢伤害值（如果玩家的伤害属性发生了变化） 
        arrow.gameObject.SetActive(true);

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
        switch (playerStatus.CurrentAttackType)
        {
            case PlayerStatus.AttackType.Attack_Horizontal:
                // 水平攻击逻辑
                attackPoint_Horizontal.gameObject.SetActive(true);
                break;
            case PlayerStatus.AttackType.Attack_Up:
                // 向上攻击逻辑
                attackPoint_Up.gameObject.SetActive(true);
                break;
            case PlayerStatus.AttackType.Attack_Down:
                // 向下攻击逻辑
                attackPoint_Down.gameObject.SetActive(true);
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
        attackPoint_Horizontal.gameObject.SetActive(false);
        attackPoint_Up.gameObject.SetActive(false);
        attackPoint_Down.gameObject.SetActive(false);

        playerStatus.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        playerStatus.CanChangeAttackType = true; // 同时解锁攻击类型切换
        playerStatus.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击

        attackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }


    /// <summary>
    /// AnimationEvent:Shoot动画结束时调用
    /// </summary>
    public void ShootEnd()
    {
        OutShootArrow(); // 生成箭矢并发射

        playerStatus.CanChangeState = true;// 解除状态锁定，允许再次切换状态
        playerStatus.CanChangeAttackType = true; // 同时解锁攻击类型切换
        playerStatus.CanAttack = false;// 射击结束后，设置CanAttack为false，防止在射击过程中再次攻击


        attackIntervalTimer.enabled = true;// 攻击间隔定时器重新启用，准备下一次攻击
    }

    /// <summary>
    /// Player受伤害的Action
    /// </summary>
    public void OnTakeDamage()
    {
        // 播放受伤害动画
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }


}