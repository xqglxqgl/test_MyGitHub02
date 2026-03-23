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

    protected virtual bool AutoCreateSingleton { get; } = true;
}