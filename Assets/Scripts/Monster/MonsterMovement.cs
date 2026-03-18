using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    

    [Header("内部组件引用")]
    [SerializeField]private AutoLockSystem autoLockSystem; // 自动锁定系统组件
    [SerializeField]private MonsterStatus monsterStatus; // 引用属性管理器
    [SerializeField]private Monster_BattleLogic monsterBattleLogic; // 引用战斗逻辑
    [SerializeField]private Rigidbody2D rb;

    private Transform playerTransform; 

    void Awake()
    {
        autoLockSystem.onLockTargetChange += UpdateTarget; // 订阅锁定事件
    }

    void Update()
    {
        monsterBattleLogic.JudgeMonsterState(); // 判断monster应该处于什么状态

        switch (monsterStatus.CurrentState)// 根据当前状态播放对应动画
        {
            case MonsterStatus.MonsterState.Idle:
                monsterBattleLogic.PlayIdleAnimation();
                break;
            case MonsterStatus.MonsterState.Run:
                monsterBattleLogic.PlayRunAnimation();
                break;
            case MonsterStatus.MonsterState.Attack:
                monsterBattleLogic.PlayAttackAnimation();
                break;
            case MonsterStatus.MonsterState.Shoot:
                monsterBattleLogic.PlayShootAnimation();
                break;
        }
    }
    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void UpdateTarget(Transform target)
    {
        playerTransform = target;
    }

    private void MoveTowardsPlayer()
    {
        // 只有在 Run 状态下才移动
        if (monsterStatus.CurrentState == MonsterStatus.MonsterState.Run)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // 根据移动方向调整怪物朝向
            Vector3 currentScale = transform.localScale;
            if (direction.x < 0)
            {
                // 目标在左边，朝向左
                currentScale.x = -Mathf.Abs(currentScale.x);
            }
            else if (direction.x > 0)
            {
                // 目标在右边，朝向右
                currentScale.x = Mathf.Abs(currentScale.x);
            }
            transform.localScale = currentScale;

            // 使用 Rigidbody 进行物理移动
            rb.MovePosition(rb.position + direction * monsterStatus.monsterProperty.moveSpeed * Time.fixedDeltaTime);
        }
    }
}