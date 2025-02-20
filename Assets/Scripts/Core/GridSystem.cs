using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;

    private string TEAM_TAG = "Player";

    private void Start()
    {
        warriorButton.onClick.AddListener(() => SpawnUnit(Unit.UnitTypeEnum.Warrior, TEAM_TAG));
        archerButton.onClick.AddListener(() => SpawnUnit(Unit.UnitTypeEnum.Archer, TEAM_TAG));
    }

    private void SpawnUnit(Unit.UnitTypeEnum unitType, string teamTag)
    {
        Tile[] spawnableTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in spawnableTiles)
        {
            if (tile.CanSpawn)
            {
                Vector3 spawnPosition = tile.transform.position + Vector3.up * 1.7f;
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject unitObject = ObjectPool.Instance.SpawnFromPool(unitType, 1, spawnPosition, rotation);

                Unit unit = unitObject.GetComponent<Unit>();
                // unit.ResetUnit();
                if (unit != null)
                {
                    unit.CurrentTile = tile;
                    tile.SetUnit(unit);
                    tile.CanSpawn = false;
                    unitObject.tag = teamTag;
                }

                Unit otherUnit = tile.GetUnit();
                if (otherUnit != null && otherUnit != unit && 
                    otherUnit.UnitType == unit.UnitType && 
                    otherUnit.UnitLevel == unit.UnitLevel)
                {
                    UnitMergeHandler.Instance.TryMergeUnits(unit, otherUnit);
                }
                break;
            }
        }
    }
}