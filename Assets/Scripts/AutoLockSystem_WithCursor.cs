using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoLockSystem_WithCursor : MonoBehaviour
{
    [Header("锁定信息")]
    [SerializeField]private float lockRange = 6f;
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private float checkInterval = 0.1f;

    [Header("光标图片引用")]
    [SerializeField]private Image cursorLeftUp;
    [SerializeField]private Image cursorRightUp;
    [SerializeField]private Image cursorLeftDown;
    [SerializeField]private Image cursorRightDown;


    public UnityAction<Transform> onLockTargetChange;
    private List<GameObject> activeMarkers = new List<GameObject>();
    private Transform currentTarget; 
    private float lastCheckTime;


    void Start()
    {
        lastCheckTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= lastCheckTime + checkInterval)
        {
            lastCheckTime = Time.time;
            FindNearestEnemy();
        }
    }

    private void FindNearestEnemy()
    {
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, lockRange, enemyLayer);
        
        if (enemies.Length == 0)
        {
            ClearTarget();
            return;
        }
        // 找到最近的敌人
        Transform nearest = null;
        float minDistance = float.MaxValue;
        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }
        if(nearest != currentTarget)
        {    currentTarget = nearest;
            onLockTargetChange?.Invoke(currentTarget);// 触发锁定事件，传递最近敌人的Transform
        }
    }

    private void CursorLock()
    {
    }

    private void ClearTarget()
    {
        currentTarget = null;
        onLockTargetChange?.Invoke(null);// 触发锁定事件，传递null
    }

}
