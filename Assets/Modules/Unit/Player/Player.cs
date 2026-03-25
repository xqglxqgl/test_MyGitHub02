using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    private GameObject view;
    private AnimationHandler animationHandler;
    private Unit target;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector2.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
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

    void Update()
    {
        JudgeFlip();
        IsRunOrIdle();
        AutoLockTarget();

        JudgeAttack();
    }
    void FixedUpdate()
    {
        UpdateMovment();
    }

    /// <summary>
    /// 根据玩家输入判断是否需要播放跑步或闲置动画
    /// </summary>
    private void IsRunOrIdle()
    {
        if (InputManager.Instance.MovementDir == Vector2.zero)
        {
            animationHandler.SetIdleOrMove(false);
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            animationHandler.SetIdleOrMove(true);
        }
    }
    /// <summary>
    /// 通过判断目标是否在攻击范围内来判断是否需要攻击
    /// </summary>
    private void JudgeAttack()
    {
        if (target == null) return;

        var distance = Vector2.Distance(this.transform.position, target.transform.position);
        var attackDir = target.transform.position - transform.position;
        if (distance <= property.attackRange)
        {
            animationHandler.PlayAttackAnimationByDir(attackDir);
        }
    }



    private void UpdateMovment()
    {
        var inputDir = InputManager.Instance.MovementDir;
        var currentPos = rigidBody.position;
        var moveDir = property.moveSpeed * Time.fixedDeltaTime * inputDir;
        rigidBody.MovePosition(currentPos + moveDir);
    }

    private void JudgeFlip()
    {
        // 如果攻击距离内有敌人则优先根据敌人位置判断是否需要翻转
        if (target != null)
        {
            var distance = Vector2.Distance(this.transform.position, target.transform.position);
            if (distance < property.attackRange + 1f)//增加1f的缓冲距离
            {
                var dir = target.transform.position - transform.position;
                if (dir.x > 0)
                {
                    animationHandler.Flip(false);
                    return;
                }
                else if (dir.x < 0)
                {
                    animationHandler.Flip(true);
                    return;
                }
            }
        }



        //没有敌人则根据输入方向判断是否需要翻转
        if (InputManager.Instance.MovementDir.x > 0)
        {
            animationHandler.Flip(false);
        }
        else if (InputManager.Instance.MovementDir.x < 0)
        {
            animationHandler.Flip(true);
        }
    }

}
