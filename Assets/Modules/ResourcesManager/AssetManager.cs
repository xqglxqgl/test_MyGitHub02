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


#region 音频资源
    //音频播放预制体
    public const string ASPrefab_BGM = "Prefabs/Audio/BGM";
    public const string ASPrefab_CombatSFX = "Prefabs/Audio/CombatSFX";
    public const string ASPrefab_UISFX = "Prefabs/Audio/UISFX";
    
    // 战斗音效
    public const string AC_Fight_ArrowHit = "Audio/Combat/Arrow_Hit";
    public const string AC_Fight_BowDraw = "Audio/Combat/Bow_Draw";
    public const string AC_Fight_BowRelease = "Audio/Combat/Bow_Release";
    public const string AC_Fight_ClubHit = "Audio/Combat/Club_Hit";
    public const string AC_Fight_ClubSwing = "Audio/Combat/Club_Swing";
    public const string AC_Fight_Heal = "Audio/Combat/Heal";
    public const string AC_Fight_Slashed = "Audio/Combat/Slashed";
    public const string AC_Fight_SwordHit = "Audio/Combat/Sword_Hit";
    public const string AC_Fight_SwordSwing = "Audio/Combat/Sword_Swing";

    //UI音效
    public const string AC_UI_BTClicky = "Audio/UI/BT_Clicky";
    public const string AC_UI_BTMuffled = "Audio/UI/BT_Muffled";
    public const string AC_UI_CorrectChime = "Audio/UI/CorrectChime";
    public const string AC_UI_ErrorChime = "Audio/UI/ErrorChime";
    public const string AC_UI_Drink = "Audio/UI/Drink";
    public const string AC_UI_HeavyJingle = "Audio/UI/HeavyJingle";
    public const string AC_UI_LightJingle = "Audio/UI/LightJingle";
    public const string AC_UI_MainMenu = "Audio/UI/MainMenu";
    public const string AC_UI_Pause = "Audio/UI/Pause";

#endregion


}