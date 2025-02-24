using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<Unit> enemies = new List<Unit>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(Unit enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Unit enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public bool AreAllEnemiesDead()
    {
        if (enemies == null || enemies.Count == 0)
        {
            return true;
        }

        return enemies.All(enemy => enemy != null && enemy.UnitHealth != null && enemy.UnitHealth.HP <= 0);
    }

}