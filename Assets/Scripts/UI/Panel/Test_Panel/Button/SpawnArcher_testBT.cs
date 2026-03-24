using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnArcher_testBT : MonoBehaviour
{
    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(SpawnObjGo);
    }
    public void SpawnObjGo()
    {
        var archer = UnitManager.Instance.CreateMonster(AssetPathUtility.UnitView_MArcher,AssetPathUtility.Property_MArcher);
    }
}
