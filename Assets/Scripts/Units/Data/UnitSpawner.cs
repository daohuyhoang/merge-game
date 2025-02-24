using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    private void Start()
    {
        SpawnPlayerUnits();
    }

    private void SpawnPlayerUnits()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.playerUnitsData.Count == 0)
        {
            Debug.LogWarning("No player units data found!");
            return;
        }

        foreach (UnitDataSave data in GameDataManager.Instance.playerUnitsData)
        {
            SpawnUnit(data);
        }
    }

    private void SpawnUnit(UnitDataSave data)
    {
        GameObject unitObject = ObjectPool.Instance.SpawnFromPool(data.unitType, data.unitLevel, data.position, data.rotation);
        if (unitObject != null)
        {
            Unit unit = unitObject.GetComponent<Unit>();
            if (unit != null)
            {
                unit.UnitLevel = data.unitLevel;
                unit.transform.position = data.position;
                unit.transform.rotation = data.rotation;
                unit.tag = "Player";
                UnitHealthBar healthBar = unitObject.GetComponentInChildren<UnitHealthBar>();
                healthBar.SetHealthBarColor();

                Tile nearestTile = unit.FindNearestTile();
                if (nearestTile != null)
                {
                    unit.CurrentTile = nearestTile;
                    nearestTile.SetUnit(unit);
                    nearestTile.CanSpawn = false;
                }

                Debug.Log($"Spawned unit: {unit.UnitType}, Level: {unit.UnitLevel}, Position: {unit.transform.position}");
            }
        }
    }
}