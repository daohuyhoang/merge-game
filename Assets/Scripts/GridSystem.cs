using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int rows = 3;
    [SerializeField] int columns = 5;
    [SerializeField] float tileSize = 1.5f;
    [SerializeField] float gridSpacing = 2.0f;
    [SerializeField] Vector3 startPosition = new Vector3(-8.1f, -1.4f, -13.5f);

    void Start()
    {
        CreateGrid(startPosition, false);
        CreateGrid(startPosition + new Vector3(0, 0, rows * tileSize + gridSpacing), true);
    }

    void CreateGrid(Vector3 startPos, bool canSpawn)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = startPos + new Vector3(x * tileSize, 0, z * tileSize);
                GameObject cell = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                
                Tile cellComponent = cell.AddComponent<Tile>();
                cellComponent.canSpawn = canSpawn;
            }
        }
    }
}