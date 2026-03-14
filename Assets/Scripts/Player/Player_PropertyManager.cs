using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_PropertyManager : MonoBehaviour
{
    // 基础状态枚举
    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
        Shoot,
        None,
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

// 主要属性(set方法需要其它的逻辑)
    // 当前状态
    private PlayerState currentState;
    public PlayerState CurrentState
    { 
        get{return currentState;} 
        set
        {        
            if (CanChangeState)
            {
                currentState = value; 

                //如果切换的是Idle或者Run/None状态则无需锁死状态切换
                if (currentState == PlayerState.Idle || currentState == PlayerState.Run||currentState == PlayerState.None) return;
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


    // 当前射击类型
    private ShootType currentShootType;
    public ShootType CurrentShootType
    { 
        get{return currentShootType;} 
        set
        {        
            if (CanChangeAttackType)
            {
                currentShootType = value; 
                CanChangeAttackType = false;
            }
        } 
    }

    // 状态控制属性(简单的get和set)
    public bool CanChangeState { get; set; }
    public bool CanChangeAttackType { get; set; }
    public bool CanAttack { get; set; }

    void Start()
    {
        if(GameManager.Instance.player.profession == Player.ProfessionType.Warrior)
        {
            InitializeProperties_Warrior();
        }
        else if(GameManager.Instance.player.profession == Player.ProfessionType.Archer)
        {
            InitializeProperties_Archer();
        }
    }


    /// ==================== 职业属性初始化方法 ====================
    void InitializeProperties_Warrior()
    {

        CanAttack = true;
        CanChangeState = true;
        CanChangeAttackType = true;
        currentState = PlayerState.Idle;
        currentAttackType = AttackType.Attack_Horizontal;   
        CanChangeState = true;
        CanChangeAttackType = true;
    }

        void InitializeProperties_Archer()
    {
        CanAttack = true;
        CanChangeState = true;
        CanChangeAttackType = true;
        currentState = PlayerState.Idle;
        currentShootType = ShootType.Shoot_Horizontal;
        CanChangeState = true;
        CanChangeAttackType = true;
    }
    
}
    


