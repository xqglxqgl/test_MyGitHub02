using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseNormal_BT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction choseNormal;


    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickChoseNormal);
    }

    private void OnClickChoseNormal()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Normal;
        choseNormal?.Invoke();
    }
}