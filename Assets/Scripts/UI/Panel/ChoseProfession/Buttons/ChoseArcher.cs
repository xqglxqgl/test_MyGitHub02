using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ChoseArcher : MonoBehaviour
{
    private Button thisButton;
    void Start()
    {
        thisButton.onClick.AddListener(OnClick);
    }

     private void OnClick()
    {
        
    }
}
