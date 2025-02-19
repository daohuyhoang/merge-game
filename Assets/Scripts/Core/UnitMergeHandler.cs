using UnityEngine;

public class UnitMergeHandler : MonoBehaviour
{
    public static UnitMergeHandler Instance;
    
    private string TEAM_TAG = "Player";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryMergeUnits(Unit unitA, Unit unitB)
    {
        if (unitA == unitB) return false;
        if (unitA.UnitType == unitB.UnitType && unitA.UnitLevel == unitB.UnitLevel)
        {
            MergeUnits(unitA, unitB);
            return true;
        }
        return false;
    }

    private void MergeUnits(Unit unitA, Unit unitB)
    {
        if (unitA.UnitLevel == unitB.UnitLevel && unitA.UnitType == unitB.UnitType)
        {
            Vector3 mergePosition = unitB.transform.position;
            int newLevel = unitA.UnitLevel + 1;
            Quaternion mergeRotation = Quaternion.Euler(0, 180, 0);
            GameObject newUnitObject = unitA.GetComponent<UnitModelHandler>().CreateHigherLevelUnit(newLevel, mergePosition, mergeRotation);
            if (newUnitObject != null)
            {
                Unit newUnit = newUnitObject.GetComponent<Unit>();
                newUnit.UnitLevel = newLevel;
                newUnit.tag = TEAM_TAG;
                newUnit.UpdateStats();
                newUnit.CurrentTile = unitB.CurrentTile;
                unitB.CurrentTile.SetUnit(newUnit);

                if (unitA.CurrentTile != null)
                {
                    unitA.CurrentTile.SetUnit(null);
                    unitA.CurrentTile.CanSpawn = true;
                }
                ObjectPool.Instance.ReturnToPool(unitA.UnitType, unitA.gameObject);
                if (unitB.CurrentTile != null)
                {
                    unitB.CurrentTile.SetUnit(newUnit);
                    unitB.CurrentTile.CanSpawn = false;
                }
                ObjectPool.Instance.ReturnToPool(unitA.UnitType, unitB.gameObject);
            }
        }
    }
}