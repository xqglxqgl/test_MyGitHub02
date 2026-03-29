using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public Property property;
    private float currentHp;
    public float CurrentHp
    {
        get { return this.currentHp; }
        set
        {
            this.currentHp = value;
            if (this.currentHp <= 0)
            {
                this.OnDie();
            }
        }
    }

    public virtual void OnDie()
    {
        Pool.Instance.Recycle(this.gameObject);
        this.gameObject.layer = LayerMask.NameToLayer("Default");// 死亡后,将游戏对象的层设置为默认层,防止单位参与游戏逻辑
    }
    public virtual void OnCreateView(string viewKey) { }
    public virtual void InitProperty(string propertyKey) { }
    public virtual void TakeDamage(float damage) { }

}
