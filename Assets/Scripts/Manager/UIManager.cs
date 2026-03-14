using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{  
    [SerializeField]private ChoseProfession choseProfession;
    [SerializeField]private ChoseDifficulty choseDifficulty;

    void Start()
    {
        choseProfession.finishChoseProfession += DisActivePanel;// 订阅选择职业事件
        choseDifficulty.finishChoseDifficulty += DisActivePanel;// 订阅选择难度事件
    }
    public void DisActivePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
