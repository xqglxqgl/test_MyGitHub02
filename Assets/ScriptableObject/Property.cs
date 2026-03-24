using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Property", menuName = "Game/Property")]

public class Property : ScriptableObject
{
    // 战斗属性
    public int level;
    public float damage;
    public float moveSpeed;
    public float attackRange;
    public int attackSpeedLevel;
    public float attackInterval;
    public float maxHp = 100;
}
