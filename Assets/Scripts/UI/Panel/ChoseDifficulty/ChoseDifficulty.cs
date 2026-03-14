using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoseDifficulty : MonoBehaviour
{
    // 难度按钮
    [SerializeField]private Button buttonEazy;
    [SerializeField]private Button buttonNormal;
    [SerializeField]private Button buttonHard;

    public UnityAction<GameObject> finishChoseDifficulty;// 选择难度  完成

    void Start()
    {
        // 绑定难度按钮点击事件
        buttonEazy.onClick.AddListener(Button_ChoseEazy);
        buttonNormal.onClick.AddListener(Button_ChoseNormal);
        buttonHard.onClick.AddListener(Button_ChoseHard);

    }

    public void Button_ChoseEazy()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Easy;
        finishChoseDifficulty?.Invoke(this.gameObject);
    }

    public void Button_ChoseNormal()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Normal;
        finishChoseDifficulty?.Invoke(this.gameObject);
    }
    
    public void Button_ChoseHard()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Hard;
        finishChoseDifficulty?.Invoke(this.gameObject);
    }
}
