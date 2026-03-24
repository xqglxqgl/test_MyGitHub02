using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreatePlayer : UIBase
{
    [SerializeField] Button warriorBtn;
    [SerializeField] Button archerBtn;
    private void Awake()
    {
        this.warriorBtn.onClick.AddListener(this.OnClickWarriorBtn);
        this.archerBtn.onClick.AddListener(this.OnClickArcherBtn);
    }

    private void OnClickWarriorBtn()
    {
        var player = UnitManager.Instance.CreatePlayer(AssetPathUtility.UnitView_Warrior);
        CameraManager.Instance.Target = player.transform;

        UIManager2.Instance.ToUI<UIGame>();
    }
    private void OnClickArcherBtn()
    {
        var player = UnitManager.Instance.CreatePlayer(AssetPathUtility.UnitView_Archer);
        CameraManager.Instance.Target = player.transform;

        UIManager2.Instance.ToUI<UIGame>();
    }
}
