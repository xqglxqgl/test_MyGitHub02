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
        CreateOnePool(AssetPathUtility.Unit_PArcher);
        CreateOnePool(AssetPathUtility.Unit_PWarrior);
        CreateOnePool(AssetPathUtility.UnitView_PArcher);
        CreateOnePool(AssetPathUtility.UnitView_PWarrior);

        CreateOnePool(AssetPathUtility.Unit_MArcher);
        CreateOnePool(AssetPathUtility.Unit_MGoblin);
        CreateOnePool(AssetPathUtility.UnitView_MArcher);
        CreateOnePool(AssetPathUtility.UnitView_MGoblin);

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