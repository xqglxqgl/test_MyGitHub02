using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ObjManager : MonoBehaviour
{
    public static ObjManager instance;

    [Header("Obj事件")]
    public UnityAction<GameObject> onObjSpawned;
    public UnityAction<GameObject> onObjDespawned;

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 生成Obj
    /// </summary>
    public void SpawnObj(GameObject obj)//调用者传入需要生成的Obj
    {
        onObjSpawned?.Invoke(obj);//最终是否真的要生成, 由调接收者自己决定
    }

    /// <summary>
    /// 销毁Obj
    /// </summary>
    public void DespawnObj(GameObject obj)//调用者传入需要销毁的Obj
    {
        onObjDespawned?.Invoke(obj);//最终是否真的要销毁, 由调接收者自己决定
    }
}
