using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMonk_testBT : MonoBehaviour
{
    private Button thisButton;
    
    [Header("要生成的所有Obj")]
    [SerializeField]private GameObject[] objPrefabs;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(SpawnObjGo);
    }
    public void SpawnObjGo()
    {
        foreach (var item in objPrefabs)
        {
            ObjManager.instance.SpawnObj(item);
        }
    }
}
