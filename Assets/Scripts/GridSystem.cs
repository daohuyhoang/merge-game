using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject archerPrefab;
    [SerializeField] Button warriorButton;
    [SerializeField] Button archerButton;

    void Start()
    {
        warriorButton.onClick.AddListener(() => SpawnUnit(warriorPrefab));
        archerButton.onClick.AddListener(() => SpawnUnit(archerPrefab));
    }

    void SpawnUnit(GameObject unitPrefab)
    {
        Tile[] spawnableTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in spawnableTiles)
        {
            if (tile.canSpawn)
            {
                Vector3 spawnPosition = tile.transform.position + Vector3.up * 3.4f;
                GameObject unitObject = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);

                UnitDragHandler unit = unitObject.GetComponent<UnitDragHandler>();
                if (unit != null)
                {
                    unit.currentTile = tile;
                    tile.SetUnit(unit);
                    tile.canSpawn = false;
                }
                UnitDragHandler otherUnit = tile.GetUnit();
                if (otherUnit != null && otherUnit != unit && 
                    otherUnit.unitType == unit.unitType && 
                    otherUnit.unitLevel == unit.unitLevel)
                {
                    UnitMergeHandler.Instance.TryMergeUnits(unit, otherUnit);
                }
                break;
            }
        }
    }
}