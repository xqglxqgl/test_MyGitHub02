using UnityEngine;

public class Player_AttackPoint : MonoBehaviour
{
    [Header("玩家属性引用")]
    [SerializeField]private PropertyHandler propertyHandler;


    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "Monster"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {

            // 尝试从碰撞的敌人对象上获取 Monster_BattleLogic 组件
            Monster_BattleLogic monster = collision.gameObject.GetComponent<Monster_BattleLogic>();

            // 如果成功获取到敌人的状态管理器组件...
            if (monster != null)
            {
                // ...则调用其 TakeDamage 方法，并将玩家的伤害值作为参数传入。
                monster.TakeDamage(propertyHandler.PlayerProperty.damage);
            }
        }
    }
}