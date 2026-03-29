using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] Rigidbody2D rigidbody;
    protected Unit target { get; set; }
    protected Vector2 attackDir { get; set; }
    protected GameObject view { get; set; }
    protected AnimationHandler animationHandler { get; set; }

    #region 重写基类Unit的方法
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
    #endregion

    protected void UpdateMovment()
    {
    }
}
