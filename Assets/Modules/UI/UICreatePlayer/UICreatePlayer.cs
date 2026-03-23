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
        throw new NotImplementedException();
    }

    private void OnClickArcherBtn()
    {
        throw new NotImplementedException();
    }
}
