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
        Debug.LogError("未实现");
    }
    private void OnClickArcherBtn()
    {
        UIManager2.Instance.ToUI<UIGame>();
        UnitManager.Instance.CreatePlayer(AssetPathUtility.UnitView_Archer);
    }
}
