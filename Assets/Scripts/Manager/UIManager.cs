using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{  
    [SerializeField]private GameObject PanelGameStart;

    public void GameStart()
    {
        PanelGameStart.SetActive(true);
    }
}
