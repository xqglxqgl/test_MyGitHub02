using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseWarrior_BT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction choseWarrior;

    [Header("Player战士的预制体引用")]
    [SerializeField]private GameObject warriorPrefab;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickChoseWarrior);
    }

    private void OnClickChoseWarrior()
    {
        PlayerManager.instance.OnSpawned(warriorPrefab,new Vector2(0, 0));
        choseWarrior?.Invoke();
    }
}