using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Start()
    {
        PreparePool();
        UIManager2.Instance.ToUI<UICreatePlayer>();
    }

    private void PreparePool()
    {
        CreateOnePool(AssetPathUtility.Unit_Player);
        CreateOnePool(AssetPathUtility.UnitView_Archer);
        CreateOnePool(AssetPathUtility.UnitView_Warrior);
    }

    private void CreateOnePool(string path)
    {
        var prefab = AssetManager.Instance.LoadAsset<GameObject>(path);
        Pool.Instance.CreatePool(path, prefab);
    }
}