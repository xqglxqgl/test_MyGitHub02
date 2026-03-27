using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Item
{
    private GameObject View;
    [SerializeField] Rigidbody2D rigidBody;
    public LayerMask TargetLayer{ get; set; }

    public float Speed { get; set; }//飞行速度
    public float Damage { get; set; }//伤害值
    public Vector2 Dir { get; set; }//飞行方向

    public Vector2 OutShootPos { get; set; }//出射位置
    public float MaxFlyDistance { get; set; }//最大飞行距离
    public int PierceCount { get; set; }//穿透次数



    public override void OnCreateView(string viewKey)
    {
        this.View = Pool.Instance.Spawn(viewKey);
        this.View.transform.SetParent(this.transform);
        this.View.transform.localPosition = Vector2.zero;
    }

    private void Die()
    {
        Pool.Instance.Recycle(this.View);
        Pool.Instance.Recycle(this.gameObject);
    }

    private void UpdateMovement()
    {
        var currentPos = rigidBody.position;
        var moveDir = Speed * Dir * Time.fixedDeltaTime;
        this.rigidBody.MovePosition(currentPos + moveDir);
    }
    void Update()
    {
        var currentPos = this.rigidBody.position;
        var distance = Vector2.Distance(currentPos, this.OutShootPos);
        if (distance > this.MaxFlyDistance)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.TargetLayer.value == (1 << collision.gameObject.layer))
        {
            collision.transform.parent.GetComponent<Unit>().TakeDamage(Damage);
            this.PierceCount--;//穿透次数减少1
            if (this.PierceCount <= 0) Die();//穿透次数为0时，箭矢消失
        }
    }
}
