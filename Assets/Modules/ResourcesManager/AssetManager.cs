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
    public const string Unit_Player = "Prefabs/Prefab_Player";
    public const string UnitView_Archer = "Prefabs/Prefab_PArcher";
    public const string UnitView_Warrior = "Prefabs/Prefab_PWarrior";
    public const string Unit_Monster = "Prefabs/Prefab_Monster";
    public const string Unit_MGoblin = "Prefabs/Prefab_MGoblin";
}