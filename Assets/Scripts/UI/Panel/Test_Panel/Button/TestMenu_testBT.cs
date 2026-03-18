using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMenu_testBT : MonoBehaviour
{
    [SerializeField]private GameObject testMenu;
    private Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOpenTestMenu);
    }

    private void OnOpenTestMenu()
    {
        testMenu.SetActive(true);
        for (int i = 0; i < testMenu.transform.childCount; i++)
        {
            testMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
