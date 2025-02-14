using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitTypeEnum
    {
        Warrior,
        Archer
    }

    [SerializeField] private UnitTypeEnum unitType = UnitTypeEnum.Warrior;

    public string UnitType => unitType.ToString();

    public int UnitLevel { get; set; } = 1;
    public int HP { get; private set; }
    public int ATK { get; private set; }
    public Tile CurrentTile { get; set; }

    [SerializeField] private UnitData unitData;

    private void Start()
    {
        UpdateStats();
        DisplayUnitInfo();
        CurrentTile = FindNearestTile();
        if (CurrentTile != null) CurrentTile.CanSpawn = false;
    }

    public void UpdateStats()
    {
        if (unitData != null && UnitLevel <= unitData.maxLevel)
        {
            HP = unitData.hpByLevel[UnitLevel - 1];
            ATK = unitData.atkByLevel[UnitLevel - 1];
        }
    }

    public Tile FindNearestTile()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        Tile nearestTile = null;
        float minDistance = Mathf.Infinity;

        foreach (Tile tile in allTiles)
        {
            float distance = Vector3.Distance(transform.position, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }
        return nearestTile;
    }
    
    public void DisplayUnitInfo()
    {
        Debug.Log($"Unit: {unitData.unitType}, Level: {UnitLevel}, HP: {HP}, ATK: {ATK}");
    }
}