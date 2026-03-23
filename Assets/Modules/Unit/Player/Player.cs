using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float speed;
    private GameObject view;

    private Animator viewAnimator;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector3.zero;

        this.viewAnimator = this.view.GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        this.UpdateMovment();

        this.UpdateAnimation();
    }
    private void UpdateMovment()
    {
        var inputDir = InputManager.Instance.NormalizedDirection;
        var moveDir = speed * Time.fixedDeltaTime * inputDir;

        var currentPos = rigidBody.position;

        rigidBody.MovePosition(currentPos + moveDir);
    }

    private void UpdateAnimation()
    {
        // this.viewAnimator
    }
}
