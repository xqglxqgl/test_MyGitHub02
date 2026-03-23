using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                _Instance = go.AddComponent<T>();
                GameObject.DontDestroyOnLoad(go);
            }
            return _Instance;
        }
    }

    // 这个是非自动创建的单例类，由场景中的实例自行提供
#warning 这个非自动创建功能还为实现
    protected virtual bool AutoCreateSingleton { get => true; }

    public void Touch() { }
}