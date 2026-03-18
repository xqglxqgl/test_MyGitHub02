using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Game/Monster")]

public class MonsterProperty : ScriptableObject
{
    // 职业枚举：战士,弓箭手,僧侣
    public enum ProfessionType
    {
        Warrior,
        Archer,
        Monk,
    }
    // 战斗属性
    public int level;
    public float damage;
    public float moveSpeed;
    public float attackRange;
    public float detectionRange;
    public int attackSpeedLevel;
    public float attackInterval;
    public float maxHealth;
    public ProfessionType profession = ProfessionType.Archer;
}
