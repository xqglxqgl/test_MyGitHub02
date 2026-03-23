using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Awake()
    {
        // 非必要
        Singleton<UnitManager>.Instance.Touch();
    }
}