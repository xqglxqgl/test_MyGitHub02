using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("攻击动画名称")]
    [SerializeField] private string attackAnimationName_Up;
    [SerializeField] private string attackAnimationName_Down;
    [SerializeField] private string attackAnimationName_Horizontal;
    [SerializeField] private string attackAnimationName_DiagonalUp;
    [SerializeField] private string attackAnimationName_DiagonalDown;

    void Start()
    {
    }
    /// <summary>
    /// 根据攻击方向播放不同的攻击动画
    /// </summary>
    public void PlayAttackAnimationByDir(Vector2 attackDir)
    {
        var angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;

        switch (angle)
        {
            case float a when a >= -30 && a < 30 || (a >= 150 || a < -150):
                animator.Play(attackAnimationName_Horizontal);
                break;
            case float a when a >= 60 && a < 120:
                animator.Play(attackAnimationName_Up);
                break;
            case float a when a >= -120 && a < -60:
                animator.Play(attackAnimationName_Down);
                break;
            case float a when a >= 30 && a < 60 || a >= 120 && a < 150:
                animator.Play(attackAnimationName_DiagonalUp);
                break;
            case float a when a >= -150 && a < -120 || a >= -60 && a < -30:
                animator.Play(attackAnimationName_DiagonalDown);
                break;
        }
    }

    public void SetIdleOrMove(bool isMove)
    {
        animator.SetBool("isRun", isMove);
    }

    public void Flip(bool isflip)
    {
        spriteRenderer.flipX = isflip;
    }

}
