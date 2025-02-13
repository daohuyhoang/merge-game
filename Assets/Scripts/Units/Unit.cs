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
    public Tile CurrentTile { get; set; }

    private UnitModelHandler modelHandler;

    private void Start()
    {
        modelHandler = GetComponent<UnitModelHandler>();
        CurrentTile = FindNearestTile();
        if (CurrentTile != null) CurrentTile.CanSpawn = false;
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
}