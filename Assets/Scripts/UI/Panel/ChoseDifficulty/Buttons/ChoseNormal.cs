using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ChoseNormal : MonoBehaviour
{
    private Button thisButton;
    public UnityAction finishClickButton;// 选择难度  完成
    void Start()
    {
        thisButton.onClick.AddListener(OnClick);
    }

     private void OnClick()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Normal;
        finishClickButton?.Invoke();
    }
}
