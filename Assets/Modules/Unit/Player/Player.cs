using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

public class Player : Unit
{
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] Transform attackPot_Right;
    [SerializeField] Transform attackPot_Left;
    [SerializeField] Transform attackPot_Up;
    [SerializeField] Transform attackPot_Down;


    private GameObject view;
    private AnimationHandler animationHandler;
    private Unit target;
#region 重写基类Unit的方法
    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector2.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
        // 初始化动画事件
        animationHandler.onShoot += OutShootArrow;
        animationHandler.onMeleeAttack += ActiveAttackPoint;
    }
    public override void InitProperty(string propertyKey)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");// 设置为Player层,确保作为Player参与游戏逻辑
        this.property = AssetManager.Instance.LoadAsset<Property>(propertyKey);
        this.CurrentHp = this.property.maxHp;
    }

    public override void TakeDamage(float damage)
    {
        animationHandler.BeHit();
        this.CurrentHp -= damage;
    }
    public override void OnDie()
    {
        base.OnDie();
        Pool.Instance.Recycle(this.view);
    }
#endregion




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



    private void OutShootArrow()
    {
        var prefab = AssetPathUtility.ItemView_ArrowP;
        var owner = transform.GetComponent<Player>();
        var offset = new Vector2(0f, -0.16f);
        var position = (Vector2)transform.position + offset;
        var dir = (target.transform.position - transform.position).normalized;
        // 生成箭矢
        var arrowGo = ItemManager.Instance.SpawnArrow(prefab);
        var arrow = arrowGo.GetComponent<Arrow>();
        // 初始化箭矢属性
        arrow.owner = owner;
        arrow.Speed = 10f;
        arrow.maxFlyDistance = 8f;
        arrow.Damage = property.damage;
        arrow.Dir = dir;
        arrow.outShootPos = position;
        arrowGo.transform.position = position;
        arrowGo.transform.right = dir;
    }

    private void ActiveAttackPoint()
    {
        var attackDir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
        switch (angle)
        {
            case float a when a >= -30 && a < 30:
                SlashUnitsInRange(attackPot_Right);
                break;
            case float a when  a >= 150 || a < -150:
                SlashUnitsInRange(attackPot_Left);
                break;
            case float a when a >= 60 && a < 120:
                SlashUnitsInRange(attackPot_Up);
                break;
            case float a when a >= -120 && a < -60:
                SlashUnitsInRange(attackPot_Down);
                break;
            case float a when a >= 30 && a < 60 || a >= 120 && a < 150:
                SlashUnitsInRange(attackPot_Up);
                break;
            case float a when a >= -150 && a < -120 || a >= -60 && a < -30:
                SlashUnitsInRange(attackPot_Down);
                break;
        }
    }

    private void SlashUnitsInRange(Transform attackPoint)
    {
        var range = 1.2f;//击打的范围,目前是写死的,后续可以根据属性动态调整
        var UnitsInRange = UnitManager.Instance.GetUnitsInRange(attackPoint.position, LayerMask.GetMask("Monster"), range);
        foreach (var unit in UnitsInRange)
        {
            unit.TakeDamage(property.damage);
        }
    }




    void OnDisable()
    {
        animationHandler.onShoot -= OutShootArrow;
    }
}
