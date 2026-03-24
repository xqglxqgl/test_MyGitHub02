using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationHandler_ForPArcher : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField]private SpriteRenderer spriteRenderer;

    void Start()
    {
    }

    void Update()
    {
        Flip();
        SetRunOrIdle();
    }

    public void PlayAnimationByInput(Vector2 dir)
    {

    }

    private void SetRunOrIdle()
    {
        if (InputManager.Instance.MovementDir == Vector2.zero)
        {
            animator.SetBool("isRun", false);
        }
        else
        {
            animator.SetBool("isRun", true);
        }
    }
    private void PlayAttack<T>(T attackType) 
    {
        switch(attackType)
        {
            case JudgeState_ForPlayerArcher.AttackType.Horizontal:
                animator.Play("Shoot_Horizontal");
                break;
            case JudgeState_ForPlayerArcher.AttackType.Up:
                animator.Play("Shoot_Up");
                break;
            case JudgeState_ForPlayerArcher.AttackType.Down:
                animator.Play("Shoot_Down");
                break;
            case JudgeState_ForPlayerArcher.AttackType.DiagonalUp:
                animator.Play("Shoot_Diagonal_Up");
                break;
            case JudgeState_ForPlayerArcher.AttackType.DiagonalDown:
                animator.Play("Shoot_Diagonal_Down");
                break;
        }
    }

    private void BeSlashed()
    {
        //被击中时,闪烁红色
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            spriteRenderer.DOColor(Color.white, 0.1f);
        });
    }

    private void BeShot()
    {
        //被击中时,闪烁红色
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            spriteRenderer.DOColor(Color.white, 0.1f);
        });
    }

    private void PlayDie()
    {
        animator.Play("Die");
    }
    //AnimationEvent: 死亡动画的最后一帧调用
    private void ReallyDie()
    {
        gameObject.SetActive(false);
    }

    private void Flip()
    {
        if(InputManager.Instance.MovementDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(InputManager.Instance.MovementDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
