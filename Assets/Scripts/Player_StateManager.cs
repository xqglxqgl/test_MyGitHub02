using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateManager : MonoBehaviour
{
    // 职业枚举：战士或弓箭手
    public enum Profession
    {
        Warrior,
        Archer
    }

    // 基础状态枚举
    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Shoot,
        Knockback
    }

    // 攻击具体类型
    public enum AttackType
    {
        Attack_Horizontal,
        Attack_Up,
        Attack_Down,
        Attack_Diagonal_Up,
        Attack_Diagonal_Down
    }

    // 射击具体类型
    public enum ShootType
    {
        Shoot_Up,
        Shoot_Down,
        Shoot_Horizontal,
        Shoot_Diagonal_Up,
        Shoot_Diagonal_Down
    }

    // --- 玩家属性 ---
    [Header("Basic Stats")] 
    [SerializeField] private int level = 1;
    [SerializeField] private Profession profession = Profession.Warrior;
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 5f;

    [Header("Upgrade Levels")]
    [SerializeField] private int attackSpeedLevel = 1;
    [SerializeField] private int knockbackLevel = 1;
    [SerializeField] private int knockbackResistanceLevel = 1;

    [Header("Current State")]
    [SerializeField] private PlayerState currentState = PlayerState.Idle;
    [SerializeField] private AttackType currentAttackType = AttackType.Attack_Horizontal;
    [SerializeField] private ShootType currentShootType = ShootType.Shoot_Horizontal;

    // 控制是否允许切换状态
    [Header("State Control")]
    [SerializeField] private bool canChangeState = true;

    // 属性访问器
    private int Level { get => level; set => level = value; }
    public Profession PlayerProfession { get => profession; set => profession = value; }  // 公开职业信息
    private float Health { get => health; set => health = value; }
    private float Damage { get => damage; set => damage = value; }
    private float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttackRange => attackRange;
    private int AttackSpeedLevel { get => attackSpeedLevel; set => attackSpeedLevel = value; }
    private int KnockbackLevel { get => knockbackLevel; set => knockbackLevel = value; }
    private int KnockbackResistanceLevel { get => knockbackResistanceLevel; set => knockbackResistanceLevel = value; }
    // 当前状态（公开读取）
    public PlayerState CurrentState { get => currentState; set => currentState = value; }
    private AttackType CurrentAttackType { get => currentAttackType; set => currentAttackType = value; }
    private ShootType CurrentShootType { get => currentShootType; set => currentShootType = value; }
    public bool CanChangeState { get => canChangeState; set => canChangeState = value; }

    // 公共方法
    /// <summary>
    /// 尝试切换到指定状态。
    /// 如果 <see cref="CanChangeState"/> 为 false，则不会改变当前状态并返回 false。
    /// </summary>
    public bool SetState(PlayerState newState)
    {
        if (!canChangeState)
            return false; // 禁止切换状态
        currentState = newState;
        return true;
    }

    /// <summary>
    /// 解除状态锁定，允许再次切换状态。
    /// 通常在床击、硬直或其他控制效果结束时调用。
    /// </summary>
    public void UnlockStateChange()
    {
        canChangeState = true;
    }

    public void SetAttackType(AttackType type) { currentAttackType = type; }
    
    public void SetShootType(ShootType type) { currentShootType = type; }
}
    


