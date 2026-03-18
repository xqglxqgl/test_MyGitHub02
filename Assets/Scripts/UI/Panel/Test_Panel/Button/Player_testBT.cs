using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_testBT : MonoBehaviour
{
    [SerializeField]private GameObject playerManu;
    private Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOpenPlayerMenu);
    }

    private void OnOpenPlayerMenu()
    {
        playerManu.SetActive(true);
        for (int i = 0; i < playerManu.transform.childCount; i++)
        {
            playerManu.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
