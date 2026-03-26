using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Unit owner = null;
    public virtual void OnCreateView(string viewKey) { }
}
