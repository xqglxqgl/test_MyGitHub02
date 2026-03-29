using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Pool : Singleton<Pool>
{
    private Dictionary<string, GameObject> prefabs;

    private Dictionary<string, Queue<GameObject>> pools;

    private Dictionary<GameObject, string> instances;

    private void Awake()
    {
        this.prefabs = new();
        this.pools = new();
        this.instances = new();
    }

    public void CreatePool(string key, GameObject prefab)
    {
        if (!this.prefabs.ContainsKey(key))
        {
            this.prefabs.Add(key, prefab);
            this.pools.Add(key, new());
        }
    }

    public GameObject Spawn(string key, Transform parent = null)
    {
        if (!this.prefabs.ContainsKey(key))
        {
            throw new System.Exception($"[Pool] {key} not found, Create Pool first");
        }

        if (this.pools[key].Count > 0)
        {
            var result = this.pools[key].Dequeue();
            this.instances.Add(result, key);
            result.SetActive(true);
            result.transform.SetParent(parent, false);
            return result;
        }
        else
        {
            var prefab = this.prefabs[key];
            var result = GameObject.Instantiate(prefab);
            result.transform.SetParent(parent, false);
            this.instances.Add(result, key);
            return result;
        }
    }

    //允许延迟回收
    public void Recycle(GameObject target, float delayTime)
    {
        StartCoroutine(RecycleDelay(target, delayTime));
    }

    public void Recycle(GameObject target)
    {
        if (this.instances.ContainsKey(target))
        {
            var key = this.instances[target];
            this.instances.Remove(target);
            this.pools[key].Enqueue(target);
            target.SetActive(false);
            target.transform.SetParent(this.transform);
        }
        else
        {
            Debug.Log($"[instances] {target.name} not found, Destroy it,不是对象池的对象!");
            // 不是对象池的对象
            GameObject.Destroy(target);
        }
    }

    //延迟回收的携程
    private IEnumerator RecycleDelay(GameObject target, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Recycle(target);
    }

}