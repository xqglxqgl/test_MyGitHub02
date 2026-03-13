using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("要监听的事件")]
    public GameEvent gameEvent;

    [Tooltip("事件触发时要执行的响应")]
    public UnityEvent onEventRaised;

    private void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        onEventRaised?.Invoke();
    }
}