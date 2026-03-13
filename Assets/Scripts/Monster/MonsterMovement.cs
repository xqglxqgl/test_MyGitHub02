using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Monster_PropertyManager monsterPropertyManager; // 引用属性管理器
    private Monster_BattleLogic monsterBattleLogic; // 引用战斗逻辑

    void Awake()
    {
        // 获取必要的组件引用
        rb = GetComponent<Rigidbody2D>();
        monsterPropertyManager = GetComponent<Monster_PropertyManager>();
        monsterBattleLogic = GetComponent<Monster_BattleLogic>();// 获取战斗逻辑组件引用

    }

    void Update()
    {
        monsterBattleLogic.JudgeMonsterState(); // 判断monster应该处于什么状态

        switch (monsterPropertyManager.CurrentState)// 根据当前状态播放对应动画
        {
            case Monster_PropertyManager.MonsterState.Idle:
                monsterBattleLogic.PlayIdleAnimation();
                break;
            case Monster_PropertyManager.MonsterState.Run:
                monsterBattleLogic.PlayRunAnimation();
                break;
            case Monster_PropertyManager.MonsterState.Attack:
                monsterBattleLogic.PlayAttackAnimation();
                break;
            case Monster_PropertyManager.MonsterState.Shoot:
                monsterBattleLogic.PlayShootAnimation();
                break;
        }
    }
    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        // 只有在 Run 状态下才移动
        if (monsterPropertyManager.CurrentState == Monster_PropertyManager.MonsterState.Run)
        {
            Vector2 direction = (monsterBattleLogic.playerTransform.position - transform.position).normalized;

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
            rb.MovePosition(rb.position + direction * monsterPropertyManager.MoveSpeed * Time.fixedDeltaTime);
        }
    }
}