using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
    private Dictionary<string, UnityEngine.Object> assetLoaded;

    private void Awake()
    {
        this.assetLoaded = new();
    }

    public T LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        if (this.assetLoaded.ContainsKey(path))
        {
            return this.assetLoaded[path] as T;
        }
        var asset = Resources.Load<T>(path);
        this.assetLoaded.Add(path, asset);
        return asset;
    }
}

public static class AssetPathUtility
{
    public const string Unit_PArcher = "Prefabs/Units/Player/P_Archer";
    public const string Unit_PWarrior = "Prefabs/Units/Player/P_Warrior";
    public const string UnitView_PArcher = "Prefabs/Units/Player/PV_Archer";
    public const string UnitView_PWarrior = "Prefabs/Units/Player/PV_Warrior";

    public const string Unit_MArcher = "Prefabs/Units/Monster/M_Archer";
    public const string Unit_MGoblin = "Prefabs/Units/Monster/M_Goblin";
    public const string UnitView_MGoblin = "Prefabs/Units/Monster/MV_Goblin";
    public const string UnitView_MArcher = "Prefabs/Units/Monster/MV_Archer";

    public const string Item_Arrow = "Prefabs/Items/Arrow/Arrow";
    public const string ItemView_ArrowP = "Prefabs/Items/Arrow/ArrowP";
    public const string ItemView_ArrowM = "Prefabs/Items/Arrow/ArrowM";



    public const string Property_PArcher = "ScriptableObject/Property_PArcher";
    public const string Property_PWarrior = "ScriptableObject/Property_PWarrior";
    public const string Property_MGoblin = "ScriptableObject/Property_MGoblin";
    public const string Property_MArcher = "ScriptableObject/Property_MArcher";




}