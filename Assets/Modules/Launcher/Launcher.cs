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
        CreateOnePool(AssetPathUtility.Unit_Monster);
        CreateOnePool(AssetPathUtility.Unit_MGoblin);
        CreateOnePool(AssetPathUtility.UnitView_MArcher);

        CreateOnePool(AssetPathUtility.Item_Arrow);
        CreateOnePool(AssetPathUtility.ItemView_ArrowP);
        CreateOnePool(AssetPathUtility.ItemView_ArrowM);
    }

    private void CreateOnePool(string path)
    {
        var prefab = AssetManager.Instance.LoadAsset<GameObject>(path);
        Pool.Instance.CreatePool(path, prefab);
    }
}