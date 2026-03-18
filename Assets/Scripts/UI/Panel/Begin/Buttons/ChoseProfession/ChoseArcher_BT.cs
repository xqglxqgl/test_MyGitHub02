using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoseArcher_BT : MonoBehaviour
{
    private Button thisButton;
    public UnityAction choseArcher;

    [Header("Player弓箭手的预制体引用")]
    [SerializeField]private GameObject archerPrefab;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickChoseArcher);
    }

    private void OnClickChoseArcher()
    {
        GameManager.Instance.OnPlayerSpawned(archerPrefab,new Vector2(0, 0));
        choseArcher?.Invoke();
    }
}