using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseHard_BT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction choseHard;


    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickChoseHard);
    }

    private void OnClickChoseHard()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Hard;
        choseHard?.Invoke();
    }
}