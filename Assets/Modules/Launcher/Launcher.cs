using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        UIManager2.Instance.ToUI<UICreatePlayer>();
    }
}