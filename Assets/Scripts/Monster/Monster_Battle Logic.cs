using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Monster_StateManager), typeof(Animator))]
public class Monster_BattleLogic : MonoBehaviour
{
    private Monster_StateManager monster;
    private Animator animator;
    private Transform playerTransform;

    private GameObject arrowPrefab;// 箭矢预制体引用
    private float arrowSpeed = 1f;    // 当前目标方向，用于决定动画

    Vector2 directionToPlayer;

    // 缓存攻击点的Transform引用
    private Transform AttackPoint_Horizontal;
    private Transform AttackPoint_Up ;
    private Transform AttackPoint_Down ;


    void Awake()
    {
        monster = GetComponent<Monster_StateManager>();
        animator = GetComponent<Animator>();

        // 缓存玩家的 Transform，避免重复查找
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        arrowPrefab = Resources.Load<GameObject>("Prefabs/ArrowPrefab02");


        // 查找子对象AttackPoint并缓存攻击点的Transform引用
        AttackPoint_Horizontal = transform.Find("AttackPoint_Horizontal");
        AttackPoint_Up = transform.Find("AttackPoint_Up");
        AttackPoint_Down = transform.Find("AttackPoint_Down");
    }

    /// <summary>
    /// 检测攻击范围内是否存在玩家，并根据怪物职业切换到攻击或射击状态。
    /// </summary>
    /// <returns>如果成功切换到攻击或射击状态，则返回 true，否则返回 false。</returns>
    private bool CheckFor_PlyaerCurrentState_AttackOrShoot()
    {
        // 使用 OverlapCircleAll 检测范围内的所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monster.AttackRange+0.5f); // 增加一个小的缓冲距离，确保检测更准确
        
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
                    directionToPlayer = (closestPlayer.position - transform.position).normalized;// 计算攻击方向
                }
            }
        }

        // 如果找到了玩家，并且玩家在攻击范围内，则根据职业切换状态
        if (closestPlayer != null &&  closestDistance <= monster.AttackRange )
        {
            // 怪物职业是战士时，切换到Attack状态
            if (monster.MonsterProfession == Monster_StateManager.Profession.Warrior)
            {
                if (monster.SetState(Monster_StateManager.MonsterState.Attack)) 
                {
                monster.CanChangeState = false;
                }
                return true;// 成功切换成Attack状态
            }

            // 职业为弓箭手时，切换到Shoot状态
            else if (monster.MonsterProfession == Monster_StateManager.Profession.Archer)
            {
                if(monster.SetState(Monster_StateManager.MonsterState.Shoot))
                {
                    monster.CanChangeState = false;
                }
                return true; // 成功切换成Shoot状态
            }
            
        }

        return false; // 未切换状态
    }

    /// <summary>
    /// 检查是否应该将怪物状态切换为跑动或待机。
    /// </summary>
    private void CheckFor_PlayerCurrentState_RunOrIdle()
    {
        // 如果找不到玩家，直接返回
        if (playerTransform == null) return;

        // 计算玩家与怪物之间的距离
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 玩家在侦测范围之外，且怪物当前不是Idle状态 -> 切换到Idle
        if (distanceToPlayer > monster.DetectionRange)
        {
            if(monster.CurrentState != Monster_StateManager.MonsterState.Idle)
            {
                if(monster.SetState(Monster_StateManager.MonsterState.Idle))
                {

                }   
            }
        }
        // 玩家在侦测范围之内，但在攻击范围之外 -> 切换到Run
        else if (distanceToPlayer > monster.AttackRange)
        {
            if(monster.CurrentState != Monster_StateManager.MonsterState.Run)
            {
                if(monster.SetState(Monster_StateManager.MonsterState.Run))
                {

                }
            }
        }
    }

    public void PlayAttackAnimation()
    {
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    // 水平攻击前，根据玩家方向调整怪物朝向
                    Vector3 currentScale = transform.localScale;
                    if (directionToPlayer.x < 0)
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

                    monster.SetAttackType(Monster_StateManager.AttackType.Attack_Horizontal);
                    animator.Play("Attack_Horizontal");
                }
                else
                {
                    if (directionToPlayer.y > 0)
                    {
                        monster.SetAttackType(Monster_StateManager.AttackType.Attack_Up);
                        animator.Play("Attack_Up");
                    }
                    else
                    {
                        monster.SetAttackType(Monster_StateManager.AttackType.Attack_Down);
                        animator.Play("Attack_Down");
                    }
                }
    }

    public void JudgeMonsterState()
    {
        if(CheckFor_PlyaerCurrentState_AttackOrShoot())return; // 优先检查攻击/射击状态
        CheckFor_PlayerCurrentState_RunOrIdle();    
        
    }

    public void PlayShootAnimation()
    {
                if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
                {
                    // 水平攻击前，根据玩家方向调整怪物朝向
                    Vector3 currentScale = transform.localScale;
                    if (directionToPlayer.x < 0)
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
                }
                animator.Play("Shoot");
    }

    public void PlayIdleAnimation()
    {
        animator.SetBool("isRun", false);
    }

    public void PlayRunAnimation()
    {
        animator.SetBool("isRun", true);
    }

    //AnimationEvent：射出箭矢
    private void OutShootArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogError("箭矢预制体引用为空，无法发射箭矢。");
            return;
        }

        // 生成箭矢在世界坐标系中，位置相对于该Monster
        Vector3 spawnPosition = transform.position + new Vector3(0.01004279f, -0.07191658f, -2.5f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        
        // 设置箭矢的飞行方向和速度，以及发射箭矢的Monster对象引用
        ArrowMovement02 arrowMovement = arrow.GetComponent<ArrowMovement02>();
        if (arrowMovement != null)
        {
            arrowMovement.SetDirection(directionToPlayer);
            arrowMovement.SetSpeed(arrowSpeed);
            arrowMovement.monster = transform; // 传入该发出箭矢的Monster对象引用，方便访问该Monster的属性
        }
        else
        {
            Debug.LogError("箭矢预制体缺少 ArrowMovement 组件，无法设置飞行方向和速度。");
        }
        // 设置箭矢朝向攻击方向（箭矢默认朝右）
        if (directionToPlayer != Vector2.zero)
        {
            // 将箭头的右端朝向攻击方向（箭头默认朝右）
            arrow.transform.right = directionToPlayer;
        }
    }

        //AnimatorEvent：Attack动画进行到一半时（刀砍下）调用
    private void Active_AttackPoint()
    {
        switch (monster.CurrentAttackType)
        {
            case Monster_StateManager.AttackType.Attack_Horizontal:
                // 水平攻击逻辑
                AttackPoint_Horizontal.gameObject.SetActive(true);
                break;
            case Monster_StateManager.AttackType.Attack_Up:
                // 向上攻击逻辑
                AttackPoint_Up.gameObject.SetActive(true);
                break;
            case Monster_StateManager.AttackType.Attack_Down:
                // 向下攻击逻辑
                AttackPoint_Down.gameObject.SetActive(true);
                break;
        }
    }

    //AnimatorEvent：Attack动画结束时（刀收回）调用
    private void Deactive_AttackPoint()
    {
        AttackPoint_Horizontal.gameObject.SetActive(false);
        AttackPoint_Up.gameObject.SetActive(false);
        AttackPoint_Down.gameObject.SetActive(false);
    }

    /// <summary>
    /// 怪物受到伤害时调用，减少生命值
     /// </summary>
    /// <param name="amount">伤害值</param>
    public void TakeDamage(float amount)
    {
        monster.Sethealth(monster.Health - amount);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }


}