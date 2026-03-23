using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // 属性
    protected float hp;
    protected float maxHp;
    protected float attack;
    protected float moveSpeed;

    // 移动
    public void Move(Vector2 direction)
    {
        Debug.Log("通用移动");
    }

    // 攻击
    public virtual void Attack() { }
}
