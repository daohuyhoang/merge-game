using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public Unit.UnitTypeEnum unitType;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<Unit.UnitTypeEnum, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<Unit.UnitTypeEnum, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.unitType, objectPool);
        }
    }

    public GameObject SpawnFromPool(Unit.UnitTypeEnum unitType, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(unitType))
        {
            Debug.LogWarning("Pool with unit type " + unitType + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[unitType].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[unitType].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(Unit.UnitTypeEnum unitType, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(unitType))
        {
            Debug.LogWarning("Pool with unit type " + unitType + " doesn't exist.");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[unitType].Enqueue(objectToReturn);
    }
}