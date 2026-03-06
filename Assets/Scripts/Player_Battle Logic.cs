using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BattleLogic : MonoBehaviour
{
    private Player_StateManager player;
    private Animator animator;
    private Vector2 attackDirection;    // 当前目标方向，用于决定动画
    private GameObject arrowPrefab;     // 箭矢预制体引用
    private float arrowSpeed = 1f;     // 箭矢速度

    void Start()
    {
        // 在自身查找 Player_StateManager 组件
        player = GetComponent<Player_StateManager>();
        if (player == null)
        {
            Debug.LogError("Player_BattleLogic 无法找到 Player_StateManager 组件，请检查挂载位置。");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Player_BattleLogic 需要一个 Animator 组件来播放攻击动画。");
            enabled = false;
            return;
        }

        // 加载箭矢预制体
        arrowPrefab = Resources.Load<GameObject>("Prefabs/ArrowPrefab");
        if (arrowPrefab == null)
        {
            Debug.LogError("无法加载 ArrowPrefab 预制体，请确保它在 Resources/Prefabs 文件夹中。");
        }

    }

    public Vector2 CheckFor_PlyaerCurrentState_AttackOrShoot()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, player.AttackRange);
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Monster"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            attackDirection = (closestEnemy.position - transform.position).normalized;

            // 对齐敌人改变攴向
            if (attackDirection.x > 0)
                transform.localScale = new Vector3(1, 1, 1);  // 面向右
            else if (attackDirection.x < 0)
                transform.localScale = new Vector3(-1, 1, 1); // 面向左

            // 根据职业判断是进行攻击还是射击
            if (player.PlayerProfession == Player_StateManager.Profession.Archer)
            {
                // 弓箤手职业：使用射击状态
                player.SetShootType(DetermineShootType());
                if (player.SetState(Player_StateManager.PlayerState.Shoot))
                {
                    PlayAttackAnimation();
                }
            }
            else
            {
                // 战士职业：使用攻击状态
                Player_StateManager.AttackType attackType = DetermineAttackType();
                player.SetAttackType(attackType);
                if (player.SetState(Player_StateManager.PlayerState.Attack))
                {
                    PlayAttackAnimation();
                }
            }

            return attackDirection;
        }
        else
        {
            if(player.SetState(Player_StateManager.PlayerState.Idle))
            {
                animator.SetBool("isRun", false);
            }
            return Vector2.zero; // 无敌人时返回零向量
        }
    }

    /// <summary>
    /// 根据攻击方向判断进行进进行攻击的类型
    /// </summary>
    private Player_StateManager.AttackType DetermineAttackType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            return Player_StateManager.AttackType.Attack_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_StateManager.AttackType.Attack_Diagonal_Up : Player_StateManager.AttackType.Attack_Up;
        }
        else
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_StateManager.AttackType.Attack_Diagonal_Down : Player_StateManager.AttackType.Attack_Down;
        }
    }

    /// <summary>
    /// 根据射击方向判断进行射击的类型
    /// </summary>
    private Player_StateManager.ShootType DetermineShootType()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            return Player_StateManager.ShootType.Shoot_Horizontal;
        }
        else if (attackDirection.y > 0)
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_StateManager.ShootType.Shoot_Diagonal_Up : Player_StateManager.ShootType.Shoot_Up;
        }
        else
        {
            return (Mathf.Abs(attackDirection.x) > 0.3f) ? Player_StateManager.ShootType.Shoot_Diagonal_Down : Player_StateManager.ShootType.Shoot_Down;
        }
    }

    /// <summary>
    /// 播放攻击或射击动画的入口方法。
    /// 它会根据玩家的当前状态决定是播放近战攻击动画还是射击动画。
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (animator == null) return;
        if (attackDirection == Vector2.zero) return;
        if (player == null) return;

        if (player.CurrentState == Player_StateManager.PlayerState.Shoot)
        {
            PlayShootAnimation();
        }
        else
        {
            PlayMeleeAttackAnimation();
        }
        player.CanChangeState = false;
    }

    private void PlayMeleeAttackAnimation()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            animator.Play("Attack_Horizontal");
        }
        else if (attackDirection.y > 0)
        {
            animator.Play("Attack_Up");
        }
        else
        {
            animator.Play("Attack_Down");
        }
    }

    private void PlayShootAnimation()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            animator.Play("Shoot_Horizontal");
        }
        else if (attackDirection.y > 0)
        {
            animator.Play(Mathf.Abs(attackDirection.x) > 0.3f ? "Shoot_Diagonal_Up" : "Shoot_Up");
        }
        else
        {
            animator.Play(Mathf.Abs(attackDirection.x) > 0.3f ? "Shoot_Diagonal_Down" : "Shoot_Down");
        }
    }

    /// <summary>
    /// AnimationEvent: 在射击动画的特定帧调用，用于实际生成并发射箭矢。
    /// </summary>
    /// <remarks>
    /// 此方法应在Unity编辑器的动画窗口中，为射击动画剪辑添加一个动画事件来调用。
    /// 它负责实例化箭矢预制体，设置其初始位置和旋转，并赋予其飞行的初速度。
    /// </remarks>
    void OutShootArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return;
        }

        // 生成箭矢在世界坐标系中，位置相对于Player
        Vector3 spawnPosition = transform.position + new Vector3(-0.011f, -0.118f, -1f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        
        ArrowMovement arrowMovement = arrow.GetComponent<ArrowMovement>();
        if (arrowMovement != null)
        {
            arrowMovement.SetDirection(attackDirection);
            arrowMovement.SetSpeed(arrowSpeed);
        }
        else
        {
            Debug.LogError("箭矢预制体缺少 ArrowMovement 组件，无法设置飞行方向和速度。");
        }
        // 设置箭矢朝向攻击方向（箭矢默认朝右）
        if (attackDirection != Vector2.zero)
        {
            // 将箭头的右端朝向攻击方向（箭头默认朝右）
            arrow.transform.right = attackDirection;
        }

    }
}