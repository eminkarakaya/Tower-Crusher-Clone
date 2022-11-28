using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }
    public Pool[] pools = null;
    private void Awake()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledObjects = new Queue<GameObject>();
            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject obj = Instantiate(pools[i].objectPrefab);
                obj.SetActive(false);
                pools[i].pooledObjects.Enqueue(obj);
            }
        }
    }
    public GameObject GetPooledObject(int objType)
    {
        if (objType >= pools.Length) return null;
        GameObject obj = pools[objType].pooledObjects.Dequeue();
        return obj;

    }
    public void SetPooledObject(GameObject pooledObject,int objectType)
    {
        if (objectType >= pools.Length) return;
        pools[objectType].pooledObjects.Enqueue(pooledObject);
        pooledObject.SetActive(false);
    }
}
