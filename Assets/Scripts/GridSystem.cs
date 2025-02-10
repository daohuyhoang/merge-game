using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int rows = 3;
    [SerializeField] int columns = 5;
    [SerializeField] float cellSize = 1.5f;
    [SerializeField] float gridSpacing = 2.0f;
    [SerializeField] Vector3 startPosition = new Vector3(-8.1f, -1.4f, -13.5f);

    void Start()
    {
        CreateGrid(startPosition);
        CreateGrid(startPosition + new Vector3(0, 0, rows * cellSize + gridSpacing));
    }

    void CreateGrid(Vector3 startPosition)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = startPosition + new Vector3(x * cellSize, 0, z * cellSize);
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}