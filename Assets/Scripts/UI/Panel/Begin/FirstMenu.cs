using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstMenu : MonoBehaviour
{

    [Header("当前菜单下的2个按钮")]
    [SerializeField]private ChoseArcher_BT choseArcher;
    [SerializeField]private ChoseWarrior_BT choseWarrior;


    private void Awake()
    {
        choseArcher.choseArcher += DisableAllChildren;
        choseWarrior.choseWarrior += DisableAllChildren;
    }

    private void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
