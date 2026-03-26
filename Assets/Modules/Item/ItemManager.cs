using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private List<Item> itemList;
    void Awake()
    {
        this.itemList = new();
    }

    public Item SpawnArrow(string viewPrefab)
    {
        var arrowGo = Pool.Instance.Spawn(AssetPathUtility.Item_Arrow);
        var arrow = arrowGo.GetComponent<Arrow>();
        arrow.OnCreateView(viewPrefab);
        this.itemList.Add(arrow);

        return arrow;
    }
}
