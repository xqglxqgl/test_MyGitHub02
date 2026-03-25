using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Item
{
    private GameObject View;
    [SerializeField]private Rigidbody2D rigidBody;

    public float speed;
    public float damage;
    public Vector2 dir;

    private float lifeTime = 2f;
    private float dieTime;

    private void Start()
    {
        dieTime = Time.time + lifeTime;
    }
    public override void OnCreateView(string viewKey)
    {
        this.View = Pool.Instance.Spawn(viewKey);
        this.View.transform.SetParent(this.transform);
        this.View.transform.localPosition = Vector2.zero;
    }
    public override void InitProperty()
    {
        transform.right = dir;//箭矢头方向指向射击方向
    }

    void Update()
    {
        if(Time.time >= dieTime)
        {
            Pool.Instance.Recycle(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        var currentPos = rigidBody.position;
        var moveDir = speed * dir * Time.fixedDeltaTime;
        rigidBody.MovePosition(currentPos + moveDir);
    }
}
