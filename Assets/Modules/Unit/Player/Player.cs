using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private GameObject view;
    private Animator viewAnimator;

    private Unit target;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector3.zero;

        this.viewAnimator = this.view.GetComponent<Animator>();
        this.spriteRenderer = this.view.GetComponent<SpriteRenderer>();
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
        SetRunOrIdle();
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
    private void SetRunOrIdle()
    {
        if (InputManager.Instance.MovementDir == Vector2.zero)
        {
            viewAnimator.SetBool("isRun", false);
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            viewAnimator.SetBool("isRun", true);
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
        if (Mathf.Abs(distance) <= property.attackRange)
        {
            PlayAttackAnimationByDir(attackDir);
        }
    }

    /// <summary>
    /// 根据攻击方向播放不同的攻击动画
    /// </summary>
    private void PlayAttackAnimationByDir(Vector2 attackDir)
    {
        var angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;

        switch (angle)
        {
            case float a when a >= -30 && a < 30 || a >= 150 && a < -150:
                viewAnimator.Play("Shoot_Horizontal");
                break;
            case float a when a >= 60 && a < 120:
                viewAnimator.Play("Shoot_Up");
                break;
            case float a when a >= -120 && a < -60:
                viewAnimator.Play("Shoot_Down");
                break;
            case float a when a >= 30 && a < 60 || a >= 120 && a < 150:
                viewAnimator.Play("Shoot_Diagonal_Up");
                break;
            case float a when a >= -150 && a < -120 || a >= -60 && a < -30:
                viewAnimator.Play("Shoot_Diagonal_Down");
                break;
        }
    }

    private void UpdateMovment()
    {
        var inputDir = InputManager.Instance.MovementDir;
        var moveDir = property.moveSpeed * Time.fixedDeltaTime * inputDir;

        var currentPos = rigidBody.position;

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
                    spriteRenderer.flipX = false;
                    return;
                }
                else if (dir.x < 0)
                {
                    spriteRenderer.flipX = true;
                    return;
                }
            }
        }



        //没有敌人则根据输入方向判断是否需要翻转
        if (InputManager.Instance.MovementDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (InputManager.Instance.MovementDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

}
