using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LastMenu : MonoBehaviour
{

    [Header("当前菜单下的3个按钮")]
    [SerializeField]private ChoseEazy_BT choseEazy;
    [SerializeField]private ChoseNormal_BT choseNormal;
    [SerializeField]private ChoseHard_BT choseHard;



    public UnityAction finish;

    private void Awake()
    {
        choseEazy.choseEazy += OnFinish;
        choseNormal.choseNormal += OnFinish;
        choseHard.choseHard += OnFinish;


    }

    private void OnFinish()
    {
        finish?.Invoke();
    }

    private void EnableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        transform.gameObject.SetActive(true);
    }

}
