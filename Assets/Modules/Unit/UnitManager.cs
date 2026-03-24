using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private List<Unit> unitList;

    private void Awake()
    {
        this.unitList = new();
    }


    public Unit CreatePlayer(string viewPrefab,string propertyKey)
    {
        var playerGo = Pool.Instance.Spawn(AssetPathUtility.Unit_Player);
        var player = playerGo.GetComponent<Player>();
        player.OnCreateView(viewPrefab);
        player.InitProperty(propertyKey);
        this.unitList.Add(player);

        return player;
    }

    public Unit CreateMonster(string viewPrefab,string propertyKey)
    {
        var monsterGo = Pool.Instance.Spawn(AssetPathUtility.Unit_Monster);
        var monster = monsterGo.GetComponent<Monster>();
        monster.OnCreateView(viewPrefab);
        monster.InitProperty(propertyKey);
        this.unitList.Add(monster);
        return monster;
    }

    public Unit GetNearestTarget(Unit self,LayerMask layer)
    {
        Unit nearestTarget = null;
        var nearestDistance = Mathf.Infinity;
        foreach (var unit in this.unitList)
        {
            if((1<<unit.gameObject.layer & layer.value) == 0)continue;
        
            var targetDistance = Vector2.Distance(self.transform.position, unit.transform.position);
            if (nearestTarget == null || targetDistance < nearestDistance)
            {
                nearestTarget = unit;
                nearestDistance = targetDistance;
            }
        }
        return nearestTarget;
    }

}