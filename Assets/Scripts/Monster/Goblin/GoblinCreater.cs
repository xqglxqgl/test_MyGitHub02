using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class GoblinCreater : MonoBehaviour
{
    private ObjectPool<GameObject> objPool;

    [Header("obj生成范围(圆环)")]
    [SerializeField]private float innerRadius = 2f;
    [SerializeField]private float outerRadius = 5f;

    private GameObject prefabSpawn;//此预制体用于,接收生成Obj事件传入的预制体,并且用于最终的Obj生成

    private Vector2 spawnPosition;


    void Awake()
    {
        
    }

    void Start()
    {
        ObjManager.instance.onObjSpawned += SpawnObj;

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

    private GameObject CreateObj()
    {
        //初始化位置
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(innerRadius, outerRadius);
        spawnPosition = (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        GameObject obj = Instantiate(prefabSpawn,spawnPosition,Quaternion.identity);

        obj.GetComponent<MonsterStatus>().onDie += OnDie;//订阅死亡事件,根据Obj的类型决定所引用的组件(这里是MonsterStatus)

        return obj;
    }
    private void OnGetObj(GameObject obj)
    {   
        //初始化位置
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(innerRadius, outerRadius);
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



    private void SpawnObj(GameObject prefab)//接收生成Obj事件所传入的预制体
    {
        if(!prefab.CompareTag("Goblin"))return;//如果不是Goblin, 则不生成(与其它Creater区分开)
        prefabSpawn = prefab;
        objPool.Get();
    }

    private void OnDie(GameObject obj)
    {
        objPool.Release(obj);//归还到对象池
    }
}
