using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private float speed;
    private GameObject view;
    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey, this.transform);
        this.view = viewInstance;

    }

    private void FixedUpdate()
    {
        var inputDir = InputManager.Instance.NormalizedDirection;
        var moveDir = speed * Time.fixedDeltaTime * inputDir;
    }
}
