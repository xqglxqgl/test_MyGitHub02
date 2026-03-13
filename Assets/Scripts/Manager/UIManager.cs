using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{    
    private GameObject Panel_ChoseProfession;
    private GameObject Panel_ChoseDifficulty;



    void Awake()
    {
        // 初始化UI元素
        Panel_ChoseProfession = GameObject.Find("Canvas/GameStart/Panel_ChoseProfession");
        Panel_ChoseDifficulty = GameObject.Find("Canvas/GameStart/Panel_ChoseDifficulty");

    }


    public void GameStart()
    { 
        Panel_ChoseProfession.SetActive(true);
        Panel_ChoseDifficulty.SetActive(false);
        Debug.Log("GameStart");
    }
}
