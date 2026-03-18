using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_testBT : MonoBehaviour
{
    [SerializeField]private GameObject monsterManu;
    private Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOpenMonsterMenu);
    }

    private void OnOpenMonsterMenu()
    {
        monsterManu.SetActive(true);
        for (int i = 0; i < monsterManu.transform.childCount; i++)
        {
            monsterManu.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
