using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_ChoseProfession : MonoBehaviour
{
    [SerializeField]private GameObject PanelChoseProfession;
    [SerializeField]private GameObject PanelChoseDifficulty;


    [SerializeField]private GameObject PlayerPrefab_Warrior;
    [SerializeField]private GameObject PlayerPrefab_Archer;

    [SerializeField]private Button button_ChoseWarrior;
    [SerializeField]private Button button_ChoseArcher;

    void Start()
    {
        button_ChoseWarrior.onClick.AddListener(Button_ChoseWarrior);
        button_ChoseArcher.onClick.AddListener(Button_ChoseArcher);
    }

    public void Button_ChoseWarrior()
    {
        GameManager.Instance.SpawnPlayer(PlayerPrefab_Warrior, Vector3.zero);
        PanelChoseProfession.SetActive(false);
        PanelChoseDifficulty.SetActive(true);
    }

    public void Button_ChoseArcher()
    {
        GameManager.Instance.SpawnPlayer(PlayerPrefab_Archer, Vector3.zero);
        PanelChoseProfession.SetActive(false);
        PanelChoseDifficulty.SetActive(true);
    }
}
