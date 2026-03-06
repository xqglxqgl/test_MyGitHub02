using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private float Speed = 1f; // 箭矢速度
    private Vector2 Direction;// 飞行方向

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
        
        // 当箭矢碰撞到怪物时，销毁箭矢并对怪物造成伤害
        if (collision.CompareTag("Monster"))
        {
            Debug.Log("检测到Monster标签，销毁箭矢");
            Destroy(gameObject);
            // 在这里添加对怪物造成伤害的逻辑
        }
    }
}