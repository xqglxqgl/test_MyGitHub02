using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTimer_AttackInterval : MonoBehaviour,ITimerUser
{
    private Monster_PropertyManager monster;
    
    private float nextAttackTime;
    private bool isActive = false;
    

    void Awake()
    {
        // 在自身查找 Monster_PropertyManager 组件
        monster = GetComponent<Monster_PropertyManager>();
        if (monster == null)
        {
            Debug.LogError("MonsterTimer_AttackInterval 无法找到 Monster_PropertyManager 组件，请检查挂载位置。");
            enabled = false;
            return;
        } 
    }
    void OnEnable()
    {
        isActive = true;
        nextAttackTime = Time.time + monster.AttackInterval;
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
            nextAttackTime = currentTime + monster.AttackInterval;
            monster.CanAttack = true;
            this.enabled = false;
        }
    }
}
