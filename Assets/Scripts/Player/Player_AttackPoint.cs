using UnityEngine;

public class Player_AttackPoint : MonoBehaviour
{
    // 缓存玩家状态管理器
    private Player_PropertyManager playerStateManager;

    private void Awake()
    {
        // 查找父对象（玩家）并获取其 Player_PropertyManager 组件的引用。
        GameObject player = transform.parent.gameObject;
        if (player != null)
        {
            playerStateManager = player.GetComponent<Player_PropertyManager>();
        }
        else
        {
            Debug.LogError("父对象中未找到 Player_PropertyManager 组件！", this);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "Monster"
        if (collision.CompareTag("Monster"))
        {

            // 尝试从碰撞的敌人对象上获取 Monster_BattleLogic 组件
            Monster_BattleLogic monster = collision.gameObject.GetComponent<Monster_BattleLogic>();

            // 如果成功获取到敌人的状态管理器组件...
            if (monster != null)
            {
                // ...则调用其 TakeDamage 方法，并将玩家的伤害值作为参数传入。
                monster.TakeDamage(playerStateManager.Damage);
            }
        }
    }
}