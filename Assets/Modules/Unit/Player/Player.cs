using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    protected Unit target { get; set; }
    protected Vector2 attackDir { get; set; }
    protected AnimationHandler animationHandler { get; set; }
    protected GameObject view { get; set; }
#region 重写基类Unit的方法
    public override void InitProperty(string propertyKey)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");// 设置为Player层,确保作为Player参与游戏逻辑
        this.property = AssetManager.Instance.LoadAsset<Property>(propertyKey);
        this.CurrentHp = this.property.maxHp;
    }
    public override void TakeDamage(float damage)
    {
        this.animationHandler.BeHit();
        this.CurrentHp -= damage;
    }

#endregion

    protected void AutoLockTarget()
    {
        // 玩家自动锁定目标
        this.target = UnitManager.Instance.GetNearestTarget(this, LayerMask.GetMask("Monster"));
        if (this.target != null)
        {
            this.attackDir = this.target.transform.position - this.transform.position;
        }
    }

    /// <summary>
    /// 根据玩家输入判断是否需要播放跑步或闲置动画
    /// </summary>
    protected void IsRunOrIdle()
    {
        if (InputManager.Instance.MovementDir == Vector2.zero)
        {
            this.animationHandler.SetIdleOrMove(false);
            this.rigidBody.velocity = Vector2.zero;
        }
        else
        {
            this.animationHandler.SetIdleOrMove(true);
        }
    }
    /// <summary>
    /// 通过判断目标是否在攻击范围内来判断是否需要攻击
    /// </summary>
    protected void JudgeAttack()
    {
        if (this.target == null) return;

        var distance = Vector2.Distance(this.transform.position, this.target.transform.position);
        if (distance <= this.property.attackRange)
        {
            this.Attack();
        }
    }

    protected void Attack()
    {
        this.animationHandler.PlayAttackAnimationByDir(this.attackDir);
    }

    protected void UpdateMovment()
    {
        var inputDir = InputManager.Instance.MovementDir;
        var currentPos = this.rigidBody.position;
        var moveDir = this.property.moveSpeed * Time.fixedDeltaTime * inputDir;
        this.rigidBody.MovePosition(currentPos + moveDir);
    }

    protected void JudgeFlip()
    {
        // 如果攻击距离内有敌人则优先根据敌人位置判断是否需要翻转
        if (this.target != null)
        {
            var distance = Vector2.Distance(this.transform.position, this.target.transform.position);
            if (distance < this.property.attackRange + 1f)//增加1f的缓冲距离
            {
                var dir = this.target.transform.position - this.transform.position;
                if (dir.x > 0)
                {
                    this.animationHandler.Flip(false);
                    return;
                }
                else if (dir.x < 0)
                {
                    this.animationHandler.Flip(true);
                    return;
                }
            }
        }



        //没有敌人则根据输入方向判断是否需要翻转
        if (InputManager.Instance.MovementDir.x > 0)
        {
            this.animationHandler.Flip(false);
        }
        else if (InputManager.Instance.MovementDir.x < 0)
        {
            this.animationHandler.Flip(true);
        }
    }





}
