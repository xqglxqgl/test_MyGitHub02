using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player", menuName = "Game/Player")]

public class PlayerProperty : ScriptableObject
{
    // 职业枚举：战士或弓箭手
    public enum ProfessionType
    {
        Warrior,
        Archer
    }
    // 战斗属性
    public int level;
    public float damage;
    public float moveSpeed;
    public float attackRange;
    public int attackSpeedLevel;
    public float attackInterval;
    public float maxHealth;
    public ProfessionType profession;
}
    


