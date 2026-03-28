using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnGoblin_testBT : MonoBehaviour
{
    private Button thisButton;
    

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(SpawnObjGo);
    }
    public void SpawnObjGo()
    {
        var goblin = UnitManager.Instance.CreateMonster(AssetPathUtility.Unit_MGoblin,AssetPathUtility.UnitView_MGoblin,AssetPathUtility.Property_MGoblin);
    }
}
