using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonManager : MonoBehaviour
{

    private GameObject Panel_ChoseProfession;
    private GameObject Panel_ChoseDifficulty;

    private GameObject PlayerPrefab_Warrior;
    private GameObject PlayerPrefab_Archer;

    private Button button_ChoseWarrior;
    private Button button_ChoseArcher;
    private Button button_Eazy;
    private Button button_Normal;
    private Button button_Hard;

    void Awake()
    {
        // 初始化UI元素
        Panel_ChoseProfession = GameObject.Find("Canvas/GameStart/Panel_ChoseProfession");
        Panel_ChoseDifficulty = GameObject.Find("Canvas/GameStart/Panel_ChoseDifficulty");

        // 加载玩家预制体
        PlayerPrefab_Warrior = Resources.Load<GameObject>("Prefabs/Player_Warrior");
        PlayerPrefab_Archer = Resources.Load<GameObject>("Prefabs/Player_Archer");


    }

    void Start()
    {
        // 初始化按钮
        button_ChoseWarrior = GameObject.Find("Button_ChoseWarrior").GetComponent<Button>();
        button_ChoseArcher = GameObject.Find("Button_ChoseArcher").GetComponent<Button>();
        button_Eazy = GameObject.Find("Button_ChoseEazy").GetComponent<Button>();
        button_Normal = GameObject.Find("Button_ChoseNormal").GetComponent<Button>();
        button_Hard = GameObject.Find("Button_ChoseHard").GetComponent<Button>();
        // 注册按钮事件
        button_ChoseWarrior.onClick.AddListener(Button_ChoseWarrior);
        button_ChoseArcher.onClick.AddListener(Button_ChoseArcher);
        button_Eazy.onClick.AddListener(Button_ChoseEazy);
        button_Normal.onClick.AddListener(Button_ChoseNormal);
        button_Hard.onClick.AddListener(Button_ChoseHard);
    }

    // 等待1秒才开始初始化按钮
    IEnumerator WaitForBottonToAppear()
    {
        yield return new WaitForSeconds(1f); // 等待一秒
    }

    public void Button_ChoseWarrior()
    {
        GameManager.Instance.SpawnPlayer(PlayerPrefab_Warrior, Vector3.zero);
        Panel_ChoseProfession.SetActive(false);
        Panel_ChoseDifficulty.SetActive(true);
    }

    public void Button_ChoseArcher()
    {
        GameManager.Instance.SpawnPlayer(PlayerPrefab_Archer, Vector3.zero);
        Panel_ChoseProfession.SetActive(false);
        Panel_ChoseDifficulty.SetActive(true);
    }

    public void Button_ChoseEazy()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Easy;
        Panel_ChoseProfession.SetActive(false);
        Panel_ChoseDifficulty.SetActive(false);
    }

    public void Button_ChoseNormal()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Normal;
        Panel_ChoseProfession.SetActive(false);
        Panel_ChoseDifficulty.SetActive(false);
    }
    
    public void Button_ChoseHard()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Hard;
        Panel_ChoseProfession.SetActive(false);
        Panel_ChoseDifficulty.SetActive(false);
    }
}
