using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowMovement : MonoBehaviour
{   
    // 箭矢生命周期计时器
    [SerializeField] private float lifetime = 2f;
    private float timer;

    // 飞行参数
    public float Speed { get; set; } = 10f; 
    public Vector2 Direction{ get; set; }
    public float Damage{ get; set; } 
    
    public IObjectPool<ArrowMovement> Pool{ get; set; } 


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
        
        // 当箭矢碰撞到怪物时，销毁箭矢并对怪物造成伤害
        if (collision.CompareTag("Monster"))
        {
            Pool.Release(this); // 将箭矢对象归还给对象池，而不是销毁
            // 对怪物造成伤害
            Monster_BattleLogic monsterBattleLogic = collision.GetComponent<Monster_BattleLogic>();
            monsterBattleLogic.TakeDamage(Damage);
        }
    }
}