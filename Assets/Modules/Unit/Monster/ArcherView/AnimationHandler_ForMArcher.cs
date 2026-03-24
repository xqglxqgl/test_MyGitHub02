using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationHandler_ForMArcher : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
    }

    void Update()
    {
        Flip();
        SetRunOrIdle();
    }

    private void SetRunOrIdle()
    {

    }
    private void PlayAttack<T>(T attackType)
    {

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
        spriteRenderer.flipX = false;
    }
}
