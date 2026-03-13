using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    
    // 存储所有需要计时器的敌人
    private List<ITimerUser> timers = new List<ITimerUser>(150);
    private Queue<ITimerUser> pendingAdd = new Queue<ITimerUser>();
    private Queue<ITimerUser> pendingRemove = new Queue<ITimerUser>();
    
    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        // 处理待添加
        while (pendingAdd.Count > 0)
        {
            timers.Add(pendingAdd.Dequeue());
        }
        
        // 处理待移除
        while (pendingRemove.Count > 0)
        {
            timers.Remove(pendingRemove.Dequeue());
        }
        
        // 更新所有计时器
        float currentTime = Time.time;
        foreach (var timer in timers)
        {
            timer.OnTimerUpdate(currentTime);
        }
    }
    
    public void RegisterTimer(ITimerUser timer)
    {
        pendingAdd.Enqueue(timer);
    }
    
    public void UnregisterTimer(ITimerUser timer)
    {
        pendingRemove.Enqueue(timer);
    }
}
