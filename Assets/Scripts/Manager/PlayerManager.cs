using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    //单例模式
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }

    //内部组件引用
    private GameObject player;
    private Movement_ForPlayer movement_ForPlayer;
    private TouchHandler_ForMove touchHandler_ForMove;
    private JudgeState_ForPlayerWar judgeState_ForPlayerWar;
    private JudgeState_ForPlayerArcher judgeState_ForPlayerArcher;
    private AnimationHandler_ForPlayerWar animationHandler_ForPlayerWar;
    private AnimationHandler_ForPlayerArcher animationHandler_ForPlayerArcher;
    private PropertyHandler propertyHandler;
    private AutoLockSystem_WithCursor autoLockSystem_WithCursor;


    //外部触发的事件
    public UnityAction<GameObject> onSpawned;




    //Hp事件
    public UnityAction<float> onTakeDamage;
    public UnityAction<float,float> onHpChanged;
    public UnityAction<bool> onHpIsLow;
    public UnityAction onHpZero;
    public UnityAction onReallyDie;//由AnimationHandler播放死亡动画之后,触发的事件

    //动画事件
    public UnityAction<bool> onMoveOrIdle;
    public UnityAction<Enum> onAttack;
    public UnityAction onBeSlashed;
    public UnityAction onBeShot;

    // 玩家移动向量改变事件, 包含移动向量和移动速度
    public UnityAction<Vector2,float> onMovementVectorChanged;

    // 玩家锁定目标改变事件, 包含锁定目标的Transform,自身的攻击距离和攻击间隔
    public UnityAction<Transform,float,float> onLockTargetChange;



    /// <summary>
    /// 生成玩家
    /// </summary>
    public void OnSpawned(GameObject playerPrefab, Vector2 position)
    {
        GameObject playerSpawned = Instantiate(playerPrefab, position, Quaternion.identity);
        onSpawned?.Invoke(playerSpawned);
        if(playerPrefab.tag == "Player_Warrior") InitPlayerWarrior(playerSpawned); //战士 初始化自身组件, 方便Manager接收事件
        else if(playerPrefab.tag == "Player_Archer") InitPlayerArcher(playerSpawned);//弓箭手 初始化自身组件, 方便Manager接收事件

    }

    /// <summary>
    /// 玩家受到伤害
    /// </summary>
    public void OnTakeDamage(float damage)
    {
        onTakeDamage?.Invoke(damage);
    }

    /// <summary>
    /// 玩家被砍
    /// </summary>
    public void OnBeSlashed()
    {
        onBeSlashed?.Invoke();
    }

    /// <summary>
    /// 玩家被射
    /// </summary>
    public void OnBeShot()
    {
        onBeShot?.Invoke();
    }





    /// <summary>
    /// 初始化Player_Warrior
    /// </summary>
    private void InitPlayerWarrior(GameObject playerSpawned)
    {
        player = playerSpawned;
        movement_ForPlayer = player.GetComponent<Movement_ForPlayer>();
        touchHandler_ForMove = player.GetComponent<TouchHandler_ForMove>();
        animationHandler_ForPlayerWar = player.GetComponent<AnimationHandler_ForPlayerWar>();
        judgeState_ForPlayerWar = player.GetComponent<JudgeState_ForPlayerWar>();
        propertyHandler = player.GetComponent<PropertyHandler>();
        autoLockSystem_WithCursor = player.GetComponent<AutoLockSystem_WithCursor>();

        //注册内部事件
        touchHandler_ForMove.onSendMovementVector += OnMovementVectorChanged;

        autoLockSystem_WithCursor.onLockTargetChange += OnLockTargetChanged;
        
        judgeState_ForPlayerWar.onAttack += OnAttack;
        judgeState_ForPlayerWar.isOnMove += OnMoveOrIdle;

        animationHandler_ForPlayerWar.reallyDie += OnReallyDie;

        propertyHandler.onHpChanged += OnHpChanged;
        propertyHandler.onHpIsLow += OnHpIsLow;
        propertyHandler.onHpZero += OnHpZero;

    }

    /// <summary>
    /// 初始化Player_Archer
    /// </summary>
    private void InitPlayerArcher(GameObject playerSpawned)
    {
        player = playerSpawned;
        movement_ForPlayer = player.GetComponent<Movement_ForPlayer>();
        touchHandler_ForMove = player.GetComponent<TouchHandler_ForMove>();
        animationHandler_ForPlayerArcher = player.GetComponent<AnimationHandler_ForPlayerArcher>();
        judgeState_ForPlayerArcher = player.GetComponent<JudgeState_ForPlayerArcher>();
        propertyHandler = player.GetComponent<PropertyHandler>();
        autoLockSystem_WithCursor = player.GetComponent<AutoLockSystem_WithCursor>();

        //注册内部事件
        touchHandler_ForMove.onSendMovementVector += OnMovementVectorChanged;

        autoLockSystem_WithCursor.onLockTargetChange += OnLockTargetChanged;
        
        judgeState_ForPlayerArcher.onAttack += OnAttack;
        judgeState_ForPlayerArcher.isOnMove += OnMoveOrIdle;

        animationHandler_ForPlayerArcher.reallyDie += OnReallyDie;

        propertyHandler.onHpChanged += OnHpChanged;
        propertyHandler.onHpIsLow += OnHpIsLow;
        propertyHandler.onHpZero += OnHpZero;
    }

    /// <summary>
    /// movementVector改变了,广播movementVector和Player的移动速度
    /// </summary>
    private void OnMovementVectorChanged(Vector2 movementVector)
    {
        float moveSpeed = propertyHandler.PlayerProperty.moveSpeed;

        onMovementVectorChanged?.Invoke(movementVector,moveSpeed);
    }

    /// <summary>
    /// 锁定目标改变了,广播锁定目标的Transform,自身的攻击距离
    /// </summary>
    private void OnLockTargetChanged(Transform lockTarget)
    {
        float attackRange = propertyHandler.PlayerProperty.attackRange;
        float attackInterval = propertyHandler.PlayerProperty.attackInterval;

        onLockTargetChange?.Invoke(lockTarget,attackRange,attackInterval);
    }

    /// <summary>
    /// 玩家移动或静止了,广播是否移动
    /// </summary>
    private void OnMoveOrIdle(bool isMove)
    {
        onMoveOrIdle?.Invoke(isMove);
    }

    /// <summary>
    /// 玩家攻击了,广播攻击类型
    /// </summary>
    public void OnAttack<T> (T attackType) where T : Enum
    {
        onAttack?.Invoke(attackType);
    }

    /// <summary>
    /// 玩家血量低
    /// </summary>
    private void OnHpChanged()
    {
        float currentHp = propertyHandler.CurrentHp;
        float maxHp = propertyHandler.PlayerProperty.maxHp;
        onHpChanged?.Invoke(currentHp,maxHp);
    }

    /// <summary>
    /// 玩家血量低
    /// </summary>
    private void OnHpIsLow(bool isLow)
    {
        onHpIsLow?.Invoke(isLow);
    }

    /// <summary>
    /// 玩家血量为0
    /// </summary>
    private void OnHpZero()
    {
        onHpZero?.Invoke();
    }

    /// <summary>
    /// 玩家死亡了,广播死亡事件
    /// </summary>
    private void OnReallyDie()
    {
        onReallyDie?.Invoke();
    }
}
