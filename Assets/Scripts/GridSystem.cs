using UnityEngine;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject unitPrefab;
    [SerializeField] int rows = 3;
    [SerializeField] int columns = 5;
    [SerializeField] float tileSize = 1.5f;
    [SerializeField] float gridSpacing = 2.0f;
    [SerializeField] Vector3 startPosition = new Vector3(-8.1f, -1.4f, -13.5f);
    [SerializeField] Button spawnButton;

    private Tile[] spawnableTiles;

    void Start()
    {
        CreateGrid(startPosition, false);
        CreateGrid(startPosition + new Vector3(0, 0, rows * tileSize + gridSpacing), true);

        spawnableTiles = GetSpawnableTiles();
        spawnButton.onClick.AddListener(SpawnUnit);
    }

    void CreateGrid(Vector3 startPos, bool canSpawn)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = startPos + new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                Tile tileComponent = tile.AddComponent<Tile>();
                tileComponent.canSpawn = canSpawn;
            }
        }
    }

    Tile[] GetSpawnableTiles()
    {
        return FindObjectsOfType<Tile>();
    }

    void SpawnUnit()
    {
        foreach (Tile tile in spawnableTiles)
        {
            if (tile.canSpawn)
            {
                Vector3 spawnPosition = tile.transform.position + Vector3.up * (2.0f - -1.4f);
                Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
                tile.canSpawn = false;
                break;
            }
        }
    }
}