using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    public static UIManager2 Instance { get; private set; }
    private UIBase CurrentUI;

    private void Awake()
    {
        Instance = this;
    }

    private string GetUIPath<T>() where T : UIBase
    {
        // Assets/Resources/UI/Prefabs/UICreatePlayer.prefab
        return $"UI/Prefabs/{typeof(T).Name}";
    }

    public void ToUI<T>() where T : UIBase
    {
        var path = GetUIPath<T>();
        var prefab = AssetManager.Instance.LoadAsset<GameObject>(path);
        // EnsurePoolCreated
        Pool.Instance.CreatePool(typeof(T).Name, prefab);

        var uiGo = Pool.Instance.Spawn(typeof(T).Name, this.transform);
        var ui = uiGo.GetComponent<UIBase>();
        this.CurrentUI = ui;

    }
}
