using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseEazy_BT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction choseEazy;


    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickChoseEazy);
    }

    private void OnClickChoseEazy()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Eazy;
        choseEazy?.Invoke();
    }
}