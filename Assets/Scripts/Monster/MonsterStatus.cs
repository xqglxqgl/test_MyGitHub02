using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class MonsterStatus : MonoBehaviour
{
    public UnityAction<GameObject> onDie;

    [Header("怪物基本属性(ScritableObject)")]
    public MonsterProperty monsterProperty;// 怪物属性(ScritableObject)

    // 基础状态枚举
    public enum MonsterState
    {
        Idle,
        Run,
        Attack,
        Shoot,
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


// 主要属性(set方法需要其它的逻辑)
    // 当前状态
    private MonsterState currentState;
    public MonsterState CurrentState
    { 
        get{return currentState;} 
        set
        {        
            if (CanChangeState)
            {
                currentState = value; 
                //如果切换的是Idle或者Run状态则无需锁死状态切换
                if (currentState == MonsterState.Idle || currentState == MonsterState.Run)return;
                CanChangeState = false;
            }
        } 
    }

    // 当前攻击类型
    private AttackType currentAttackType;
    public AttackType CurrentAttackType
    { 
        get{return currentAttackType;} 
        set
        {        
            if (CanChangeAttackType)
            {
                currentAttackType = value; 
                CanChangeAttackType = false;
            }
        } 
    }



    // 当前生命值
    private float currentHealth;
    public float CurrentHealth
    {
        get{return currentHealth;}

        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // 状态控制属性(简单的get和set)
    public bool CanChangeState { get; set; }
    public bool CanChangeAttackType { get; set; }
    public bool CanAttack { get; set; }


    void OnEnable()
    {
        InitializeStatus();
    }


    /// <summary>
    /// Monster死亡逻辑
    /// </summary>
    private void Die()
    {
        onDie?.Invoke(this.gameObject);//触发死亡事件,由此对象所处的Creater监听并且回收此对象
    }




    /// <summary>
    /// 初始化状态
    /// </summary>
    void InitializeStatus()
    {
        // 初始化血量和所有状态
        CurrentHealth = monsterProperty.maxHealth;
        CanAttack = true;
        CanChangeState = true;
        CanChangeAttackType = true;
        currentState = MonsterState.Idle;
        currentAttackType = AttackType.Attack_Horizontal;   
        CanChangeState = true;
        CanChangeAttackType = true;
    }

    
}
    


