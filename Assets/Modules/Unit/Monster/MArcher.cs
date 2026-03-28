using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MArcher : Monster
{
#region 重写基类Unit的方法
    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector3.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
    }
    public override void OnDie()
    {
        base.OnDie();
        Pool.Instance.Recycle(this.view);
    }
#endregion
}
