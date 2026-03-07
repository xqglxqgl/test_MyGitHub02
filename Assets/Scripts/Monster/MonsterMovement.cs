using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Monster_StateManager monsterStateManager; // 引用状态管理器
    private Monster_BattleLogic monsterBattleLogic; // 引用战斗逻辑

    void Awake()
    {
        // 获取必要的组件引用
        rb = GetComponent<Rigidbody2D>();
        monsterStateManager = GetComponent<Monster_StateManager>();
        monsterBattleLogic = GetComponent<Monster_BattleLogic>();// 获取战斗逻辑组件引用

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("场景中未找到标签为 'Player' 的对象，请检查玩家对象的标签设置。", gameObject);
        }
    }

    void Update()
    {
        monsterBattleLogic.JudgeMonsterState(); // 判断monster应该处于什么状态

        switch (monsterStateManager.CurrentState)// 根据当前状态播放对应动画
        {
            case Monster_StateManager.MonsterState.Idle:
                monsterBattleLogic.PlayIdleAnimation();
                break;
            case Monster_StateManager.MonsterState.Run:
                monsterBattleLogic.PlayRunAnimation();
                break;
            case Monster_StateManager.MonsterState.Attack:
                monsterBattleLogic.PlayAttackAnimation();
                break;
            case Monster_StateManager.MonsterState.Shoot:
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
        if (playerTransform != null && monsterStateManager.CurrentState == Monster_StateManager.MonsterState.Run)
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
            rb.MovePosition(rb.position + direction * monsterStateManager.MoveSpeed * Time.fixedDeltaTime);
        }
    }
}