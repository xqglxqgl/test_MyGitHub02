using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JudgeState_ForPlayerArcher : MonoBehaviour
{
    private Vector2 movementVector;

    /// 锁定目标相关变量
    private Transform target;
    private float distanceToTarget;
    private Vector2 attackDirection;

    /// 攻击相关变量
    private float attackInterval;
    private float lastAttackTime;
    private float attackRange;

    /// 角色面朝方向相关变量
    private float localScaleX;
    private Vector2 originalScale;


    public UnityAction<AttackType> onAttack;
    public UnityAction<bool> isOnMove;




    public enum AttackType
    {
        Up,
        Down,
        Horizontal,
        DiagonalUp,
        DiagonalDown,
    }
    void Awake()
    {
        lastAttackTime = Time.time;
        localScaleX = transform.localScale.x;
        originalScale = transform.localScale;
    }

    void Start()
    {
        PlayerManager.instance.onMovementVectorChanged += ReciveMoveData;
        PlayerManager.instance.onLockTargetChange += ChangeTarget;
    }

    void Update()
    {
        AssessFacingDirection();
        JudgeNeedMoveOrIdle();
        JudgeNeedAttack();
    }


    private void ReciveMoveData(Vector2 movementVector, float moveSpeed)
    {
        this.movementVector = movementVector;
    }
    private void ChangeTarget(Transform lockTarget, float attackRange, float attackInterval)
    {
        target = lockTarget;
        if (target == null) return;

        this.attackRange = attackRange;
        this.attackInterval = attackInterval;

        attackDirection = target.position - transform.position;
        distanceToTarget = Vector2.Distance(transform.position, target.position);
    }

    /// <summary>
    /// 判断是否需要移动
    /// </summary>
    private void JudgeNeedMoveOrIdle()
    {
        if (movementVector != Vector2.zero)
        {
            isOnMove?.Invoke(true);
        }
        else
        {
            isOnMove?.Invoke(false);
        }
    }

    /// <summary>
    /// 判断是否需要攻击
    /// </summary>
    private void JudgeNeedAttack()
    {
        if (Time.time < lastAttackTime + attackInterval) return;//如果距离上次攻击时间小于攻击间隔,则不攻击
        if (target == null) return;//如果没有目标,则不攻击

        if (distanceToTarget <= attackRange)
        {
            onAttack?.Invoke(AssessAttackType());
            lastAttackTime = Time.time;
        }
    }

    /// <summary>
    /// 评估并且返回攻击类型
    /// </summary>
    private AttackType AssessAttackType()
    {
        if (attackDirection.y > 0)
        {   
            if (attackDirection.y < Mathf.Abs(attackDirection.x) * 0.3)
            return AttackType.Horizontal;
            else if (attackDirection.y > Mathf.Abs(attackDirection.x) * 0.3 && attackDirection.y < Mathf.Abs(attackDirection.x) * 0.65)
            return AttackType.DiagonalUp;
            else return AttackType.Up;
        }


        if (attackDirection.y <= 0)
        {   
            if (-attackDirection.y < Mathf.Abs(attackDirection.x) * 0.3)
            return AttackType.Horizontal;
            else if (attackDirection.y > Mathf.Abs(attackDirection.x) * 0.3 && attackDirection.y < Mathf.Abs(attackDirection.x) * 0.65)
            return AttackType.DiagonalDown;
        }
        return AttackType.Down;
    }


    /// <summary>
    /// 评估角色的水平面朝方向
    /// </summary>
    private void AssessFacingDirection()
    {
        //如果有目标,则根据目标方向评估角色朝方向
        if (target != null)
        {
            if (target.position.x < transform.position.x)
            {
                originalScale.x = -localScaleX;
                transform.localScale = originalScale;
                originalScale.x = localScaleX;//恢复原始缩放,方便下次用
                return;
            }
            else transform.localScale = originalScale;
            return;
        }

        //如果没有目标,则根据移动方向评估角色朝方向
        if (movementVector.x < 0)
        {
            originalScale.x = -localScaleX;
            transform.localScale = originalScale;
            originalScale.x = localScaleX;//恢复原始缩放,方便下次用
        }
        else transform.localScale = originalScale;
    }
}
