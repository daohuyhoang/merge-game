using UnityEngine;

public class UnitMergeHandler : MonoBehaviour
{
    public static UnitMergeHandler Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryMergeUnits(UnitDragHandler unitA, UnitDragHandler unitB)
    {
        if (unitA == unitB) return false;
        if (unitA.unitType == unitB.unitType && unitA.unitLevel == unitB.unitLevel)
        {
            MergeUnits(unitA, unitB);
            return true;
        }
        return false;
    }

    private void MergeUnits(UnitDragHandler unitA, UnitDragHandler unitB)
    {
        if (unitA.unitLevel == unitB.unitLevel)
        {
            Vector3 mergePosition = unitB.transform.position;
            int newLevel = unitA.unitLevel + 1;
            GameObject newUnitObject = unitA.GetComponent<UnitModelHandler>().CreateHigherLevelUnit(newLevel, mergePosition);

            if (newUnitObject != null)
            {
                UnitDragHandler newUnit = newUnitObject.GetComponent<UnitDragHandler>();
                newUnit.unitLevel = newLevel;
                newUnit.unitType = unitA.unitType;
                newUnit.currentTile = unitA.currentTile;
                unitA.currentTile.SetUnit(newUnit);
                
                Destroy(unitA.gameObject);
                Destroy(unitB.gameObject);
            }
        }
    }
}