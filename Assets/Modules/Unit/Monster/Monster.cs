using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    [SerializeField] Rigidbody2D rigidbody;
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
    }
    private void UpdateMovment()
    {
    }
}
