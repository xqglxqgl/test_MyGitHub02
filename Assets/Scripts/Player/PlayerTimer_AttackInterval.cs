using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer_AttackInterval : MonoBehaviour, ITimerUser
{
    private Player_PropertyManager Player;
    
    private float nextAttackTime;
    private bool isActive = false;
    

    void Awake()
    {
        // 在自身查找 Player_PropertyManager 组件
        Player = GetComponent<Player_PropertyManager>();
        if (Player == null)
        {
            Debug.LogError("Player_BattleLogic 无法找到 Player_PropertyManager 组件，请检查挂载位置。");
            enabled = false;
            return;
        } 
    }
    void OnEnable()
    {
        isActive = true;
        nextAttackTime = Time.time + Player.AttackInterval;
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
            nextAttackTime = currentTime + Player.AttackInterval;
            Player.CanAttack = true;
            this.enabled = false;
        }
    }
}
