using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseProfession : MonoBehaviour
{


    [SerializeField]private GameObject playerPrefabWarrior;
    [SerializeField]private GameObject playerPrefabArcher;

    [SerializeField]private Button buttonChoseWarrior;
    [SerializeField]private Button buttonChoseArcher;
    public UnityAction<GameObject> finishChoseProfession;// 选择职业  完成

    void Start()
    {
        // 绑定职业按钮点击事件
        buttonChoseWarrior.onClick.AddListener(Button_ChoseWarrior);
        buttonChoseArcher.onClick.AddListener(Button_ChoseArcher);
    }

    public void Button_ChoseWarrior()
    {
        GameManager.Instance.SpawnPlayer(playerPrefabWarrior, Vector3.zero);
        finishChoseProfession?.Invoke(this.gameObject);
    }

    public void Button_ChoseArcher()
    {
        GameManager.Instance.SpawnPlayer(playerPrefabArcher, Vector3.zero);
        finishChoseProfession?.Invoke(this.gameObject);
    }
}
