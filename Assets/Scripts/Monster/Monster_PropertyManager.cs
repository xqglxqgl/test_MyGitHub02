using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_PropertyManager : MonoBehaviour
{
    // 职业枚举：战士或弓箭手
    public enum ProfessionType
    {
        Warrior,
        Archer
    }

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


    // 所属职业
    [SerializeField]
    private ProfessionType profession;
    public ProfessionType Profession
    { 
        get{return profession;} 
        set{profession = value;}
    }

    // 基础属性(简单的get和set)
    public int Level { get; set; }
    public float Damage { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackRange { get; set; }
    public int AttackSpeedLevel { get; set; }
    public float DetectionRange { get; set; }
    public float AttackInterval { get; set; }

    // 状态控制属性(简单的get和set)
    public bool CanChangeState { get; set; }
    public bool CanChangeAttackType { get; set; }
    public bool CanAttack { get; set; }

    void Start()
    {
        if (profession == ProfessionType.Warrior)
        {
            InitializeProperties_Warrior();
        }
        else if (profession == ProfessionType.Archer)
        {
            InitializeProperties_Archer();
        }
    }


    /// <summary>
    /// Monster死亡逻辑
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
        
    }




    /// ==================== 职业属性初始化方法 ====================
    void InitializeProperties_Warrior()
    {
        // 初始化属性
        Level = 1;
        CurrentHealth = 100f;
        Damage = 1f;
        MoveSpeed = 1f;
        AttackRange = 0.8f;
        DetectionRange = 6f;
        AttackSpeedLevel = 0;
        AttackInterval = 1.5f;
//        profession = ProfessionType.Warrior;

        CanAttack = true;
        CanChangeState = true;
        CanChangeAttackType = true;
        currentState = MonsterState.Idle;
        currentAttackType = AttackType.Attack_Horizontal;   
        CanChangeState = true;
        CanChangeAttackType = true;
    }

        void InitializeProperties_Archer()
    {
        // 初始化属性
        Level = 1;
        CurrentHealth = 100f;
        Damage = 1f;
        MoveSpeed = 1f;
        AttackRange = 3f;
        DetectionRange = 6f;
        AttackSpeedLevel = 0;
        AttackInterval = 2f;
 //       profession = ProfessionType.Archer;

        CanAttack = true;
        CanChangeState = true;
        CanChangeAttackType = true;
        currentState = MonsterState.Idle;
        CanChangeState = true;
        CanChangeAttackType = true;
    }
    
}
    


