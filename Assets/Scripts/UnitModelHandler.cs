using UnityEngine;

public class UnitModelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] unitModels;
    private GameObject currentModel;

    public void SetUnitModel(int level)
    {
        if (currentModel != null) Destroy(currentModel);

        if (level - 1 < unitModels.Length)
        {
            currentModel = Instantiate(unitModels[level - 1], transform.position, Quaternion.identity);
            currentModel.transform.SetParent(transform);
            currentModel.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Không có unit cho cấp độ này: " + level);
        }
    }

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