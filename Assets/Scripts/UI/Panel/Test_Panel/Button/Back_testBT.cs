using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Back_testBT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction back;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(BackToLastMenu);
    }

    private void BackToLastMenu()
    {
        back?.Invoke();
    }
}
