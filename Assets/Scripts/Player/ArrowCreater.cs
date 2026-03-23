using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowCreater : MonoBehaviour
{
    private ObjectPool<GameObject> objPool;

    [Header("玩家的属性管理器")]
    [SerializeField]private PropertyHandler propertyHandler;

    [Header("玩家的箭矢预制体")]
    [SerializeField]private GameObject prefabSpawn;

    private Vector2 spawnPosition;
    private Vector2 attackDirection;

    private Vector2 spawnPositionOffset = new Vector2(0, -0.15f);


    void Start()
    {
        PlayerManager.instance.onLockTargetChange += UpdateTarget;

        // 创建对象池
        objPool = new ObjectPool<GameObject>(
        createFunc: CreateObj,           // 创建新对象的方法
        actionOnGet: OnGetObj,           // 从池中取出时的回调
        actionOnRelease: OnReleaseObj,   // 归还时的回调
        actionOnDestroy: OnDestroyObj,   // 销毁时的回调（池满时）
        collectionCheck: false,
        defaultCapacity: 15,
        maxSize: 40
        );
    }

    private void UpdateTarget(Transform lockTarget, float attackRange, float attackInterval)
    {
        if (lockTarget == null) return;
        
        attackDirection = (lockTarget.position - transform.position).normalized;
    }


    private GameObject CreateObj()
    {
        GameObject obj = Instantiate(prefabSpawn);
        InitArrowPositionAndData(obj);
        obj.GetComponent<ArrowMovement>().onRelease += OnDie;//订阅死亡事件,根据Obj的类型决定所引用的组件(这里是ArrowMovement)

        return obj;
    }
    private void OnGetObj(GameObject obj)
    {   
        InitArrowPositionAndData(obj);
        obj.transform.position = spawnPosition;

        obj.gameObject.SetActive(true);
    }



    private void OnReleaseObj(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void OnDestroyObj(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    private void OnDie(GameObject obj)
    {
        objPool.Release(obj);//归还到对象池
    }




    private void InitArrowPositionAndData(GameObject arrow)
    {
        spawnPosition = (Vector2)transform.position + spawnPositionOffset;//设置箭矢生成位置为玩家位置+偏移量
        arrow.transform.rotation = Quaternion.identity; // 每次发射时重置箭矢旋转角度
        arrow.transform.right = attackDirection; // 每次发射时调整箭矢朝向攻击方向
        arrow.GetComponent<ArrowMovement>().direction = attackDirection;
        arrow.GetComponent<ArrowMovement>().damage = propertyHandler.PlayerProperty.damage;
    }

    //AnimationEvent: 射出箭矢
    private void OutShootArrow()
    {
        objPool.Get();
    }
}
