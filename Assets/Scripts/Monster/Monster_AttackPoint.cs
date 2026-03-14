using UnityEngine;

public class Monster_AttackPoint : MonoBehaviour
{
    [SerializeField] private Monster_PropertyManager monster;

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
        if (collision.CompareTag("Player"))
        {
            // 对Player造成伤害
            GameManager.Instance.PlayerTakeDamage(monster.Damage);// 调用GameManager的方法处理伤害,广播Action
        }
    }
}