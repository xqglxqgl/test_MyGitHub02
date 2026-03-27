using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] float speed;
    
    private GameObject view;
    private AnimationHandler animationHandler;
#region 重写基类Unit的方法
    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector3.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
    }
    public override void InitProperty(string propertyKey)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Monster");// 设置为Mosnter层,确保作为Monster参与游戏逻辑
        this.property = AssetManager.Instance.LoadAsset<Property>(propertyKey);
        this.CurrentHp = this.property.maxHp;
    }

    public override void TakeDamage(float damage)
    {
        animationHandler.BeHit();
        this.CurrentHp -= damage;
    }

    public override void OnDie()
    {
        base.OnDie();
        Pool.Instance.Recycle(this.view);
    }
#endregion

    private void FixedUpdate()
    {
        this.UpdateMovment();
    }
    private void UpdateMovment()
    {
    }
}
