using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<UnitManager>
{
    private List<Item> itemList;
    void Awake()
    {
        this.itemList = new();
    }

    public Item CreateArrow(string viewPrefab, Vector2 position,float damage,float speed,Vector2 dir)
    {
        var arrowGo = Pool.Instance.Spawn(AssetPathUtility.Item_Arrow);
        var arrow = arrowGo.GetComponent<Arrow>();
        arrow.OnCreateView(viewPrefab);
        arrow.InitProperty();
        arrow.transform.position = position;
        arrow.damage = damage;
        arrow.speed = speed;
        arrow.dir = dir;
        this.itemList.Add(arrow);

        return arrow;
    }
}
