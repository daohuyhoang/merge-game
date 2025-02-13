using UnityEngine;

public class UnitModelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] unitModels;
    private GameObject currentModel;

    public GameObject CreateHigherLevelUnit(int level, Vector3 position)
    {
        if (level - 1 < unitModels.Length)
        {
            GameObject newUnit = Instantiate(unitModels[level - 1], position, Quaternion.identity);
            return newUnit;
        }
        else
        {
            Debug.LogWarning("Không có unit cho cấp độ này: " + level);
            return null;
        }
    }
}