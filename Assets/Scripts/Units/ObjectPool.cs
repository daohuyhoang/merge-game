using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public Unit.UnitTypeEnum unitType;
        public List<GameObject> prefabs;
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
                foreach (var prefab in pool.prefabs)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }

            poolDictionary.Add(pool.unitType, objectPool);
        }
    }

    public GameObject SpawnFromPool(Unit.UnitTypeEnum unitType, int level, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(unitType))
        {
            return null;
        }
        
        GameObject objectToSpawn = poolDictionary[unitType].FirstOrDefault(obj => obj.GetComponent<Unit>().UnitLevel == level && !obj.activeInHierarchy);

        if (objectToSpawn == null)
        {
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

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
    
    public bool AreAllUnitsInactive()
    {
        foreach (var pool in poolDictionary.Values)
        {
            if (pool.Any(obj => obj.activeInHierarchy))
            {
                return false;
            }
        }
        return true;
    }
}