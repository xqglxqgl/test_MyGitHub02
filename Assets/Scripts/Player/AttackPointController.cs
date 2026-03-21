using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AttackPointController : MonoBehaviour
{
    private JudgeState_ForPlayerWar.AttackType attackType;

    [Header("攻击点")]
    [SerializeField]private GameObject attackPoint_Horizontal;
    [SerializeField]private GameObject attackPoint_Up;
    [SerializeField]private GameObject attackPoint_Down;

    void Start()
    {
        PlayerManager.instance.onAttack += UpdateAttackType;
    }

    private void UpdateAttackType(JudgeState_ForPlayerWar.AttackType attackType)
    {
        this.attackType = attackType;
    }

    //AnimationEvent: 挥刀的瞬间激活攻击点
    private void ActiveAttackPoint()
    {
        switch (attackType)
        {
            case JudgeState_ForPlayerWar.AttackType.Horizontal:
                attackPoint_Horizontal.SetActive(true);
                break;
            case JudgeState_ForPlayerWar.AttackType.Up:
                attackPoint_Up.SetActive(true);
                break;
            case JudgeState_ForPlayerWar.AttackType.Down:
                attackPoint_Down.SetActive(true);
                break;
        }
    }

    //AnimationEvent: 挥刀结束瞬间禁用所有攻击点
    private void DisableAttackPoint()
    {
        attackPoint_Horizontal.SetActive(false);
        attackPoint_Up.SetActive(false);
        attackPoint_Down.SetActive(false);
    }
}
