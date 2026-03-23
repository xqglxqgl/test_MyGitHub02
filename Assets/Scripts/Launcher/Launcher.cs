using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Awake()
    {
        Singleton<UnitManager>.Instance.Touch();
    }
}