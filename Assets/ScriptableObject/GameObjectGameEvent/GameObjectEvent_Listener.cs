using UnityEngine;
using UnityEngine.Events;

public class GameObjectEvent_Listener : MonoBehaviour
{
    [Tooltip("要监听的事件")]
    public GameObjectEvent gameObjectEvent;

    [Tooltip("事件触发时要执行的响应")]
    public UnityEvent<GameObject> onEventRaised;

    private void OnEnable()
    {
        if (gameObjectEvent != null)
            gameObjectEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameObjectEvent != null)
            gameObjectEvent.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject Player)
    {
        onEventRaised?.Invoke(Player);
    }
}