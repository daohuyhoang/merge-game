using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;

    private void Start()
    {
        warriorButton.onClick.AddListener(() => SpawnUnit(warriorPrefab));
        archerButton.onClick.AddListener(() => SpawnUnit(archerPrefab));
    }

    private void SpawnUnit(GameObject unitPrefab)
    {
        Tile[] spawnableTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in spawnableTiles)
        {
            if (tile.CanSpawn)
            {
                Vector3 spawnPosition = tile.transform.position + Vector3.up * 1.7f;
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject unitObject = Instantiate(unitPrefab, spawnPosition, rotation);

                Unit unit = unitObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.CurrentTile = tile;
                    tile.SetUnit(unit);
                    tile.CanSpawn = false;
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