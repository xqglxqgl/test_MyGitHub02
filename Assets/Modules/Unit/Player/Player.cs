using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float speed;
    private GameObject view;
    private Animator viewAnimator;
    private AnimationHandler_ForPArcher animationHandler;

    private Unit target;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector3.zero;

        this.viewAnimator = this.view.GetComponent<Animator>();
        this.animationHandler = this.view.GetComponent<AnimationHandler_ForPArcher>();
    }
    public override void InitProperty(string propertyKey)
    {
        this.property = AssetManager.Instance.LoadAsset<Property>(propertyKey);
        this.currentHp = this.property.maxHp;
    }

    private void AutoLockTarget()
    {
        // 玩家自动锁定目标
        target = UnitManager.Instance.GetNearestTarget(this, LayerMask.GetMask("Monster"));
    }

    private void FixedUpdate()
    {
        this.UpdateMovment();
        this.animationHandler.PlayAnimationByInput(InputManager.Instance.MovementDir);
    }
    private void UpdateMovment()
    {
        var inputDir = InputManager.Instance.MovementDir;
        var moveDir = speed * Time.fixedDeltaTime * inputDir;

        var currentPos = rigidBody.position;

        rigidBody.MovePosition(currentPos + moveDir);
    }

}
