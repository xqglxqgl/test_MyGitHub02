using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTimer_AttackInterval : MonoBehaviour,ITimerUser
{
    [Header("内部组件引用")]
    [SerializeField] private MonsterStatus monsterStatus;
    
    private float nextAttackTime;
    private bool isActive = false;

    private float attackInterval;
    

    void Awake()
    {
        attackInterval = monsterStatus.monsterProperty.attackInterval;
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
            monsterStatus.CanAttack = true;
            this.enabled = false;
        }
    }
}
