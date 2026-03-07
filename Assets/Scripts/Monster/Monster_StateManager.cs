using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_StateManager : MonoBehaviour
{
    // ==================== 状态定义 ====================
    public enum Profession
    {
        Warrior, // 战士
        Archer   // 弓箭手
    }

    public enum MonsterState
    {
        Idle,      // 待机
        Run,       // 移动
        Attack,    // 攻击
        Shoot,     // 射击
        Knockback  // 被击退
    }

    public enum AttackType
    {
        None,
        Attack_Horizontal,
        Attack_Up,
        Attack_Down
    }

    // ==================== 核心属性 ====================
    [Header("基础属性")]
    [SerializeField] private int level = 1;
    [SerializeField] private Profession monsterProfession = Profession.Warrior;
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("战斗属性")]
    [SerializeField] private float attackSpeedLevel = 1f;
    [SerializeField] private float knockbackPower = 1f; // 击退能力
    [SerializeField] private float knockbackResistance = 1f; // 抗击退能力
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float detectionRange = 10f; // 侦测范围

    // ==================== 状态属性 ====================
    public MonsterState CurrentState { get; private set; }
    public AttackType CurrentAttackType { get; private set; }
    public bool CanChangeState { get; set; }

    void Awake()
    {
        // 初始化状态
        CurrentState = MonsterState.Idle;
        CurrentAttackType = AttackType.None;
        CanChangeState = true;
    }

    // ==================== 属性访问器 ====================
    public int Level => level;
    public Profession MonsterProfession => monsterProfession;
    public float Health => health;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float AttackSpeedLevel => attackSpeedLevel;
    public float KnockbackPower => knockbackPower;
    public float KnockbackResistance => knockbackResistance;
    public float AttackRange => attackRange;
    public float DetectionRange => detectionRange;

    // ==================== 状态管理方法 ====================

    /// <summary>
    /// 设置怪物的新状态
    /// </summary>
    /// <param name="newState">要切换到的新状态</param>
    public bool SetState(MonsterState newState)
    {
        // 检查是否可以改变状态
        if (!CanChangeState) return false;

        if (CurrentState == newState) return false;
        
        CurrentState = newState;
        return true; // 成功切换状态

    }

    /// <summary>
    /// 设置当前的攻击类型
    /// </summary>
    /// <param name="newAttackType">新的攻击类型</param>
    public void SetAttackType(AttackType newAttackType)
    {
        CurrentAttackType = newAttackType;
    }


    public void Sethealth(float newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 怪物死亡逻辑
    /// </summary>
    private void Die()
    {
        // 在这里处理死亡逻辑，例如播放死亡动画、掉落物品、从场景中移除等
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    /// <summary>
    /// [供动画事件调用] 解锁状态机，允许状态切换。
    /// </summary>
    public void EnableStateChange()
    {
        CanChangeState = true;
    }
}