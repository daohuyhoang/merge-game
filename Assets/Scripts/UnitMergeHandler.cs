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
        if (unitA.unitType == unitB.unitType && unitA.unitLevel == unitB.unitLevel)
        {
            MergeUnits(unitA, unitB);
            return true;
        }
        return false;
    }

    private void MergeUnits(UnitDragHandler unitA, UnitDragHandler unitB)
    {
        unitA.UpgradeUnit();
        Destroy(unitB.gameObject);
    }
}