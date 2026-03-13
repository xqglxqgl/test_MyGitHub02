using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    // 选择难度面板
    [SerializeField]private GameObject PanelChoseProfession;
    [SerializeField]private GameObject PanelChoseDifficulty;
    
    // 难度按钮
    [SerializeField]private Button button_Eazy;
    [SerializeField]private Button button_Normal;
    [SerializeField]private Button button_Hard;

    void Start()
    {
        button_Eazy.onClick.AddListener(Button_ChoseEazy);
        button_Normal.onClick.AddListener(Button_ChoseNormal);
        button_Hard.onClick.AddListener(Button_ChoseHard);
    }

    public void Button_ChoseEazy()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Easy;
        PanelChoseProfession.SetActive(false);
        PanelChoseDifficulty.SetActive(false);
    }

    public void Button_ChoseNormal()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Normal;
        PanelChoseProfession.SetActive(false);
        PanelChoseDifficulty.SetActive(false);
    }
    
    public void Button_ChoseHard()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Hard;
        PanelChoseProfession.SetActive(false);
        PanelChoseDifficulty.SetActive(false);
    }
}
