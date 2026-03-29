using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWarrior : Player
{
    [Header("攻击点")]
    [SerializeField] Transform attackPot_Right;
    [SerializeField] Transform attackPot_Left;
    [SerializeField] Transform attackPot_Up;
    [SerializeField] Transform attackPot_Down;

    public override void OnCreateView(string viewKey)
    {
        var viewInstance = Pool.Instance.Spawn(viewKey);
        this.view = viewInstance;
        this.view.transform.SetParent(this.transform, false);
        this.view.transform.localPosition = Vector2.zero;

        this.animationHandler = this.view.GetComponent<AnimationHandler>();
        // 初始化动画事件
        this.animationHandler.onMeleeAttack += ActiveAttackPoint;
    }

    public override void OnDie()
    {
        base.OnDie();
        this.animationHandler.onMeleeAttack -= ActiveAttackPoint;
        Pool.Instance.Recycle(this.view);
    }
    private void ActiveAttackPoint()
    {
        var angle = Mathf.Atan2(this.attackDir.y, this.attackDir.x) * Mathf.Rad2Deg;
        switch (angle)
        {
            case float a when a >= -30 && a < 30:
                SlashUnitsInRange(this.attackPot_Right);
                break;
            case float a when a >= 150 || a < -150:
                SlashUnitsInRange(this.attackPot_Left);
                break;
            case float a when a >= 60 && a < 120:
                SlashUnitsInRange(this.attackPot_Up);
                break;
            case float a when a >= -120 && a < -60:
                SlashUnitsInRange(this.attackPot_Down);
                break;
            case float a when a >= 30 && a < 60 || a >= 120 && a < 150:
                SlashUnitsInRange(this.attackPot_Up);
                break;
            case float a when a >= -150 && a < -120 || a >= -60 && a < -30:
                SlashUnitsInRange(this.attackPot_Down);
                break;
        }
    }

    private void SlashUnitsInRange(Transform attackPoint)
    {
        var UnitsInRange = UnitManager.Instance.GetUnitsInRange(attackPoint.position, LayerMask.GetMask("Monster"), this.property.damageRange);
        foreach (var unit in UnitsInRange)
        {
            unit.TakeDamage(this.property.damage);
        }
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

}
