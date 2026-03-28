using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PArcher : Player
{
    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector2.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
        // 初始化动画事件
        this.animationHandler.onShoot += OutShootArrow;
    }

    public override void OnDie()
    {
        base.OnDie();
        this.animationHandler.onShoot -= OutShootArrow;
        Pool.Instance.Recycle(this.view);
    }

    private void OutShootArrow()
    {
        var prefab = AssetPathUtility.ItemView_ArrowP;
        var offset = new Vector2(0f, -0.16f);
        var position = (Vector2)this.transform.position + offset;
        var dir = this.attackDir.normalized;
        // 生成箭矢
        var arrow = (Arrow)ItemManager.Instance.SpawnArrow(prefab);
        // 初始化箭矢属性
        arrow.owner = this;
        arrow.TargetLayer = LayerMask.GetMask("MonsterView");
        arrow.Speed = this.property.arrowSpeed;
        arrow.MaxFlyDistance = this.property.arrowMaxFlyDistance;
        arrow.PierceCount = this.property.pierceCount;
        arrow.Damage = this.property.damage;
        arrow.Dir = dir;
        arrow.OutShootPos = position;
        arrow.transform.position = position;
        arrow.transform.right = dir;
    }

}
