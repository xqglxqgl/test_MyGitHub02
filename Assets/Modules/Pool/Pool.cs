using System.Collections.Generic;
using UnityEngine;

public class Pool : Singleton<Pool>
{
    private Dictionary<string, GameObject> prefabs;

    private Dictionary<string, Queue<GameObject>> pools;

    private Dictionary<GameObject, string> instances;

    private void Awake()
    {
        this.prefabs = new();
    }

    public void CreatePool(string key, GameObject prefab)
    {
        if (!this.prefabs.ContainsKey(key))
        {
            this.prefabs.Add(key, prefab);
            this.pools.Add(key, new());
        }
    }

    public GameObject Spawn(string key)
    {
        if (!this.prefabs.ContainsKey(key))
        {
            throw new System.Exception($"[Pool] {key} not found, Create Pool first");
        }

        if (this.pools[key].Count > 0)
        {
            var result = this.pools[key].Dequeue();
            this.instances.Add(result, key);
            return result;
        }
        else
        {
            var prefab = this.prefabs[key];
            var result = GameObject.Instantiate(prefab);
            this.instances.Add(result, key);
            return result;
        }
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
            // 不是对象池的对象
            GameObject.Destroy(target);
        }
    }
}