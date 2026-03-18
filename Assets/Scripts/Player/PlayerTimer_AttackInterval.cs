using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer_AttackInterval : MonoBehaviour, ITimerUser
{
    
    private float nextAttackTime;
    private bool isActive = false;
    [Header("自身组件引用")]
    [SerializeField]private PlayerStatus playerState;

    private float attackInterval;
    

    void Awake()
    {
        attackInterval = playerState.playerProperty.attackInterval;
    }
    void OnEnable()
    {
        isActive = true;
        nextAttackTime = Time.time + attackInterval;
        TimeManager.Instance.RegisterTimer(this);
    }


    void OnDisable()
    {
        isActive = false;
        TimeManager.Instance.UnregisterTimer(this);
    }


    public void OnTimerUpdate(float currentTime)
    {
        if (!isActive) return;
        
        if (currentTime >= nextAttackTime)
        {
            nextAttackTime = currentTime + attackInterval;
            playerState.CanAttack = true;
            this.enabled = false;
        }
    }
}
