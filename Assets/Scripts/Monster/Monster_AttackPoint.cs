using UnityEngine;

public class Monster_AttackPoint : MonoBehaviour
{
    [Header("怪物状态引用")]
    [SerializeField] private MonsterStatus monsterStatus;

    private void Awake()
    {

    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "Player"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 对Player造成伤害
            PlayerManager.instance.OnTakeDamage(monsterStatus.monsterProperty.damage);
            PlayerManager.instance.OnBeSlashed();
        }
    }
}