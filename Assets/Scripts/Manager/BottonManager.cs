using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonManager : MonoBehaviour
{
    void Awake()
    {
        // 初始化UI元素
        // Panel_ChoseProfession = GameObject.Find("Canvas/GameStart/Panel_ChoseProfession");
        // Panel_ChoseDifficulty = GameObject.Find("Canvas/GameStart/Panel_ChoseDifficulty");

        // // 加载玩家预制体
        // PlayerPrefab_Warrior = Resources.Load<GameObject>("Prefabs/Player_Warrior");
        // PlayerPrefab_Archer = Resources.Load<GameObject>("Prefabs/Player_Archer");

        // 注册按钮事件


    }

    // void Start()
    // {
    //     // 初始化按钮
    //     button_ChoseWarrior = GameObject.Find("Button_ChoseWarrior").GetComponent<Button>();
    //     button_ChoseArcher = GameObject.Find("Button_ChoseArcher").GetComponent<Button>();
    //     button_Eazy = GameObject.Find("Button_ChoseEazy").GetComponent<Button>();
    //     button_Normal = GameObject.Find("Button_ChoseNormal").GetComponent<Button>();
    //     button_Hard = GameObject.Find("Button_ChoseHard").GetComponent<Button>();
    //     // 注册按钮事件
    //     button_ChoseWarrior.onClick.AddListener(Button_ChoseWarrior);
    //     button_ChoseArcher.onClick.AddListener(Button_ChoseArcher);
    //     button_Eazy.onClick.AddListener(Button_ChoseEazy);
    //     button_Normal.onClick.AddListener(Button_ChoseNormal);
    //     button_Hard.onClick.AddListener(Button_ChoseHard);
    // }

    // 等待1秒才开始初始化按钮
    IEnumerator WaitForBottonToAppear()
    {
        yield return new WaitForSeconds(1f); // 等待一秒
    }




}
