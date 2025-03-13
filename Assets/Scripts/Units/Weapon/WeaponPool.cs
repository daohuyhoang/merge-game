using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    public static WeaponPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject projectile = Instantiate(pool.prefab);
                projectile.SetActive(false);
                objectPool.Enqueue(projectile);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnProjectile(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> projectilePool = poolDictionary[tag];

        if (projectilePool.Count == 0)
        {
            Debug.LogWarning($"Pool with tag {tag} is empty! Consider increasing pool size.");
            return null;
        }

        GameObject projectile = projectilePool.Dequeue();

        projectile.SetActive(true);
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;

        projectilePool.Enqueue(projectile);

        return projectile;
    }

    public void ReturnProjectileToPool(string tag, GameObject projectile)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            projectile.SetActive(false);
            poolDictionary[tag].Enqueue(projectile);
        }
        else
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
        }
    }
}