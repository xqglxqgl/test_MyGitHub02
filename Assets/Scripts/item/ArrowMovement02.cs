using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement02 : MonoBehaviour
{
    private float Speed = 1f; // 箭矢速度
    private Vector2 Direction;// 飞行方向

    public Transform monster{ get; set; } // 发射箭矢的Monster对象,由生成此箭矢的Monster传入引用

    // 公共方法：设置飞行方向
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    // 公共方法：设置飞行速度
    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        // 移动箭矢
        transform.Translate(Direction * Speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // 当箭矢碰撞到Player时，销毁箭矢并对Player造成伤害
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            // 对Player造成伤害
            Player_BattleLogic playerBattleLogic = collision.GetComponent<Player_BattleLogic>();
            Monster_StateManager monsterStateManager = monster.GetComponent<Monster_StateManager>();
            playerBattleLogic.TakeDamage(monsterStateManager.Damage);
        }
    }
}