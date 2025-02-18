using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<Unit> unitPool = new Queue<Unit>();

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

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject unitObj = Instantiate(unitPrefab, transform);
            Unit unit = unitObj.GetComponent<Unit>();
            unitObj.SetActive(false);
            unitPool.Enqueue(unit);
        }
    }

    public Unit GetUnitInPool()
    {
        if (unitPool.Count > 0)
        {
            Unit unit = unitPool.Dequeue();
            unit.gameObject.SetActive(true);
            return unit;
        }
        else
        {
            GameObject unitObj = Instantiate(unitPrefab, transform);
            Unit unit = unitObj.GetComponent<Unit>();
            return unit;
        }
    }

    public void ReturnUnit(Unit unit)
    {
        unit.gameObject.SetActive(false);
        unitPool.Enqueue(unit);
    }
}
