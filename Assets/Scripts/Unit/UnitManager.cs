using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private List<Unit> unitList;

    private GameObject test;
    private void Awake()
    {
        this.unitList = new();
    }

    public GameObject CreateEnemyArcher()
    {
        var prefab = AssetManager.Instance.LoadAsset<GameObject>(AssetPathUtility.Archer);

        Debug.Log($"Create Archer {prefab}");
        var unitGo = GameObject.Instantiate(prefab);
        // var unit = unitGo.GetComponent<Unit>();
        // this.unitList.Add(unit);

        return unitGo;
    }

}