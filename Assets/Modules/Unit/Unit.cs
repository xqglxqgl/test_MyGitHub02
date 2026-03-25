using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public Property property;
    public float currentHp;
    public virtual void OnCreateView(string viewKey) { }
    public virtual void InitProperty(string propertyKey){}
}
