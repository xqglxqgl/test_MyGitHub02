using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New GameEvent", menuName = "Game/GameEvent")]
public class GameEvent : ScriptableObject
{
    // 监听这个事件的监听器列表
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // 触发事件
    public void Raise()
    {
        // 从后向前遍历，防止在响应事件时修改列表
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    // 注册监听器
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    // 注销监听器
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}