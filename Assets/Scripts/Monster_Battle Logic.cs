using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Monster_StateManager), typeof(Animator))]
public class Monster_BattleLogic : MonoBehaviour
{
    private Monster_StateManager monsterStateManager;
    private Animator animator;
    private Transform playerTransform;

    void Awake()
    {
        monsterStateManager = GetComponent<Monster_StateManager>();
        animator = GetComponent<Animator>();

        // 缓存玩家的 Transform，避免重复查找
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 优先级 1: 检测攻击/射击
        if (CheckFor_PlyaerCurrentState_AttackOrShoot()) return;

        // 优先级 2: 检测是否应该追击（Run）或返回待机（Idle）
        CheckFor_PlayerCurrentState_RunOrIdle();
    }

    /// <summary>
    /// 检测攻击范围内是否存在玩家，并根据怪物职业切换到攻击或射击状态。
    /// </summary>
    /// <returns>如果成功切换到攻击或射击状态，则返回 true，否则返回 false。</returns>
    public bool CheckFor_PlyaerCurrentState_AttackOrShoot()
    {
        // 使用 OverlapCircleAll 检测范围内的所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monsterStateManager.AttackRange);
        
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                { 
                    closestDistance = distance;
                    closestPlayer = hit.transform;
                }
            }
        }

        // 如果找到了玩家，并且怪物当前不处于攻击/射击状态，则发起攻击
        if (closestPlayer != null && 
            monsterStateManager.CurrentState != Monster_StateManager.MonsterState.Attack && 
            monsterStateManager.CurrentState != Monster_StateManager.MonsterState.Shoot)
        {
            // 根据怪物职业决定攻击类型和动画
            if (monsterStateManager.MonsterProfession == Monster_StateManager.Profession.Warrior)
            {
                // 锁定状态机，防止攻击动画被打断
                monsterStateManager.CanChangeState = false;

                // 1. 计算攻击方向
                Vector2 direction = (closestPlayer.position - transform.position).normalized;

                // 2. 根据方向决定攻击类型并播放动画
                Monster_StateManager.AttackType attackType;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // 水平攻击前，根据玩家方向调整怪物朝向
                    Vector3 currentScale = transform.localScale;
                    if (direction.x < 0)
                    {
                        // 玩家在左边，朝向左
                        currentScale.x = -Mathf.Abs(currentScale.x);
                    }
                    else
                    {
                        // 玩家在右边，朝向右
                        currentScale.x = Mathf.Abs(currentScale.x);
                    }
                    transform.localScale = currentScale;

                    attackType = Monster_StateManager.AttackType.Attack_Horizontal;
                    animator.Play("Attack_Horizontal");
                }
                else
                {
                    if (direction.y > 0)
                    {
                        attackType = Monster_StateManager.AttackType.Attack_Up;
                        animator.Play("Attack_Up");
                    }
                    else
                    {
                        attackType = Monster_StateManager.AttackType.Attack_Down;
                        animator.Play("Attack_Down");
                    }
                }

                // 3. 在状态管理器中记录攻击类型和主状态
                monsterStateManager.SetAttackType(attackType);
                monsterStateManager.SetState(Monster_StateManager.MonsterState.Attack);
            }


            // 职业为弓箭手时，切换到射击状态
            else if (monsterStateManager.MonsterProfession == Monster_StateManager.Profession.Archer)
            {
                monsterStateManager.CanChangeState = false;
                if(monsterStateManager.SetState(Monster_StateManager.MonsterState.Shoot))
                {
                    animator.Play("Shoot");
                }
            }
            return true; // 成功切换状态
        }

        return false; // 未找到玩家，未切换状态
    }

    /// <summary>
    /// 检查是否应该将怪物状态切换为跑动或待机。
    /// </summary>
    public void CheckFor_PlayerCurrentState_RunOrIdle()
    {
        // 如果找不到玩家，直接返回
        if (playerTransform == null) return;

        // 计算玩家与怪物之间的距离
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 玩家在侦测范围之外，且怪物当前不是Idle状态 -> 切换到Idle
        if (distanceToPlayer > monsterStateManager.DetectionRange)
        {
            if(monsterStateManager.CurrentState != Monster_StateManager.MonsterState.Idle)
            {
                if(monsterStateManager.SetState(Monster_StateManager.MonsterState.Idle))
                {
                    animator.SetBool("isRun", false); // 确保跑动动画被关闭
                }   
            }
        }
        // 玩家在侦测范围之内，但在攻击范围之外 -> 切换到Run
        else if (distanceToPlayer > monsterStateManager.AttackRange)
        {
            if(monsterStateManager.CurrentState != Monster_StateManager.MonsterState.Run)
            {
                if(monsterStateManager.SetState(Monster_StateManager.MonsterState.Run))
                {
                    animator.SetBool("isRun", true); // 确保跑动动画被开启
                }
            }
        }
    }

}