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
    public int pierceCount = 1;//穿透次数
    public float arrowSpeed = 10f;//箭矢飞行速度
    public float arrowMaxFlyDistance = 8f;//箭矢最大飞行距离
    public float damageRange = 1.5f;//伤害范围
}
