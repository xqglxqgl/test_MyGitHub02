using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]private Back_testBT backButton;
    private void Awake()
    {
        backButton.back += DisableAllChildren;
    }

    public void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.gameObject.SetActive(false);
    }
}
