using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New GameObjectEvent", menuName = "Game/GameObjectEvent")]
public class GameObjectEvent : ScriptableObject
{
    // 监听这个事件的监听器列表
    private List<GameObjectEvent_Listener> listeners = new List<GameObjectEvent_Listener>();

    // 触发事件
    public void Raise(GameObject Player)
    {
        // 从后向前遍历，防止在响应事件时修改列表
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(Player);
        }
    }

    // 注册监听器
    public void RegisterListener(GameObjectEvent_Listener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    // 注销监听器
    public void UnregisterListener(GameObjectEvent_Listener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}