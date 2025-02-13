using UnityEngine;

[RequireComponent(typeof(UnitModelHandler))]
public class Unit : MonoBehaviour
{
    public int UnitLevel { get; set; } = 1;
    public string UnitType { get; set; }
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