using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private List<Unit> unitList;

    private void Awake()
    {
        this.unitList = new();
    }


    public Unit CreatePlayer(string viewPrefab)
    {
        var playerGo = Pool.Instance.Spawn(AssetPathUtility.Unit_Player);
        var player = playerGo.GetComponent<Player>();
        player.OnCreateView(viewPrefab);
        this.unitList.Add(player);

        return player;
    }

}