using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ArrowMovement : MonoBehaviour
{   
    // 箭矢生命周期计时器
    [SerializeField] private float lifetime = 2f;
    private float timer;

    // 飞行参数
    private float speed = 10f; 
    public Vector2 direction;
    public float damage;
    

    public UnityAction<GameObject> onRelease;


    void OnEnable()
    {
        timer = 0f;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            onRelease?.Invoke(gameObject);// 将箭矢对象归还给对象池
        }
    }

    void FixedUpdate()
    {
        // 移动箭矢
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // 当箭矢碰撞到怪物时，销毁箭矢并对怪物造成伤害
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            onRelease?.Invoke(gameObject); // 将箭矢对象归还给对象池
            // 对怪物造成伤害
            Monster_BattleLogic monsterBattleLogic = collision.GetComponent<Monster_BattleLogic>();
            monsterBattleLogic.TakeDamage(damage);
        }
    }
}