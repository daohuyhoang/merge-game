using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    public static WeaponPool Instance;

    public GameObject projectilePrefab;
    public int poolSize = 10;

    private Queue<GameObject> projectilePool;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        projectilePool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    public GameObject SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        if (projectilePool.Count == 0)
        {
            Debug.LogWarning("Projectile pool is empty! Consider increasing pool size.");
            return null;
        }

        GameObject projectile = projectilePool.Dequeue();

        projectile.SetActive(true);
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;

        projectilePool.Enqueue(projectile);

        return projectile;
    }

    public void ReturnProjectileToPool(Projectile projectile)
    {
        if (projectile != null)
        {
            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile.gameObject);
        }
    }
}