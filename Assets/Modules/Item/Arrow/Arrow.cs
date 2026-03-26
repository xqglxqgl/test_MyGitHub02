using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Item
{
    private GameObject View;
    [SerializeField] private Rigidbody2D rigidBody;

    public float Speed { get; set; }
    public float Damage { get; set; }
    public Vector2 Dir { get; set; }

    public Vector2 outShootPos;
    public float maxFlyDistance;



    public override void OnCreateView(string viewKey)
    {
        this.View = Pool.Instance.Spawn(viewKey);
        this.View.transform.SetParent(this.transform);
        this.View.transform.localPosition = Vector2.zero;
    }

    void Update()
    {
        var currentPos = rigidBody.position;
        var distance = Vector2.Distance(currentPos, outShootPos);
        if (distance > maxFlyDistance)
        {
            Pool.Instance.Recycle(this.View);
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
        var moveDir = Speed * Dir * Time.fixedDeltaTime;
        rigidBody.MovePosition(currentPos + moveDir);
    }
}
