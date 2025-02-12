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
                Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
                tile.canSpawn = false;
                break;
            }
        }
    }
}