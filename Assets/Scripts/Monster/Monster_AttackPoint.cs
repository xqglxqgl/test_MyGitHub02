using UnityEngine;

public class Monster_AttackPoint : MonoBehaviour
{
    // 缓存Monster状态管理器，避免在每次触发时都进行查找，提高性能。
    private Monster_PropertyManager monsterPropertyManager;

    private void Awake()
    {
        // 查找父对象（怪物）并获取其 Monster_StateManager 组件的引用。
        GameObject monster = transform.parent.gameObject;
        if (monster != null)
        {
            monsterPropertyManager = monster.GetComponent<Monster_PropertyManager>();
        }
        else
        {
            Debug.LogError("父对象中未找到 Monster_PropertyManager 组件！", this);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "Player"
        if (collision.CompareTag("Player"))
        {
            // 尝试从碰撞的敌人对象上获取 Player_BattleLogic 组件
            Player_BattleLogic player = collision.gameObject.GetComponent<Player_BattleLogic>();

            // 如果成功获取到敌人的状态管理器组件...
            if (player != null)
            {
                // ...则调用其 TakeDamage 方法，并将怪物的伤害值作为参数传入。
                player.TakeDamage(monsterPropertyManager.Damage);
            }
        }
    }
}