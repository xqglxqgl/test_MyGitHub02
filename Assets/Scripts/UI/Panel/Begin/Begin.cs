using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    [Header("最后一个菜单")]
    [SerializeField]private LastMenu lastMenu;
    void Awake()
    {
        lastMenu.finish += DisableAllChildren;
    }

    private void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.gameObject.SetActive(false);
    }
}
