using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : Singleton<UIManager2>
{
    public void ToUI<T>() where T : UIBase
    {

    }
}

public class UIBase : MonoBehaviour
{
    public virtual void OnShow() { }

    public virtual void OnHide() { }
}
