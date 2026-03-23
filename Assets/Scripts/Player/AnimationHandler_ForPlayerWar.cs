using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class AnimationHandler_ForPlayerWar : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public UnityAction reallyDie;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerManager.instance.onMoveOrIdle += SetRunOrIdle;
        PlayerManager.instance.onAttack += PlayAttack;
        PlayerManager.instance.onBeSlashed += BeSlashed;
        PlayerManager.instance.onBeShot += BeShot;
        PlayerManager.instance.onHpZero += PlayDie;
    }

    private void SetRunOrIdle(bool isMove)
    {
        animator.SetBool("isRun", isMove);
    }
    private void PlayAttack<T>(T attackType) where T : Enum
    {
        switch(attackType)
        {
            case JudgeState_ForPlayerWar.AttackType.Horizontal:
                animator.Play("Attack_Horizontal");
                break;
            case JudgeState_ForPlayerWar.AttackType.Up:
                animator.Play("Attack_Up");
                break;
            case JudgeState_ForPlayerWar.AttackType.Down:
                animator.Play("Attack_Down");
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
        reallyDie?.Invoke();
        gameObject.SetActive(false);
    }
}
