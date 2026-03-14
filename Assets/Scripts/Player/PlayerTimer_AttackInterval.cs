using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer_AttackInterval : MonoBehaviour, ITimerUser
{
    
    private float nextAttackTime;
    private bool isActive = false;
    private Player player;
    [SerializeField]private Player_PropertyManager playerState;
    

    void Awake()
    {
        player = GameManager.Instance.player;
    }
    void OnEnable()
    {
        isActive = true;
        nextAttackTime = Time.time + player.attackInterval;
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
            nextAttackTime = currentTime + player.attackInterval;
            playerState.CanAttack = true;
            this.enabled = false;
        }
    }
}
