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

public class Archer : Unit
{
    public override void Attack()
    {
        Debug.Log("实际的攻击逻辑-弓箭手");
    }
}

public class Goblin : Unit
{
    public override void Attack()
    {
        Debug.Log("实际的攻击逻辑-哥布林");
    }
}

public class UnitManager : MonoBehaviour
{
    private List<Unit> unitList;

    public Unit CreatePlayer(Vector2 bornPos)
    {
        Unit player = default;
        // 创建玩家
        this.unitList.Add(player);
        return player;
    }

    public Unit CreateEnemy(Vector2 bornPos)
    {
        Unit enemy = default;
        // 创建敌人
        this.unitList.Add(enemy);
        return enemy;
    }

    public Unit GetClosestUnit(Vector2 position)
    {
        Unit closestUnit = default;
        // 找的逻辑
        foreach (Unit unit in this.unitList)
        {
            // 
        }

        return closestUnit;
    }

    public void DoAttack()
    {
        Archer a = default;
        Goblin g = default;
        a.Attack();
        g.Attack();

        // 更好的写法是
        Unit unit = default;
        // 无需要在意他是什么
        unit.Attack();
    }

}