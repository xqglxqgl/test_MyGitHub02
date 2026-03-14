using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowMovement02 : MonoBehaviour
{

    // 箭矢生命周期计时器
    [SerializeField] private float lifetime = 5f;
    private float timer;

    //箭矢属性
    public float Speed { get; set; } = 3f; // 箭矢速度
    public float Damage { get; set; } // 箭矢伤害
    public Vector2 Direction{ get; set; }// 飞行方向
    public IObjectPool<ArrowMovement02> Pool { get; set; }// 箭矢对象池引用

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            // 返回对象池（不是销毁）
            Pool.Release(this);
        }
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
            Pool.Release(this); // 返回对象池

            // 对Player造成伤害
            GameManager.Instance.PlayerTakeDamage(Damage);// 调用GameManager的方法处理伤害,广播Action
        }
    }
}