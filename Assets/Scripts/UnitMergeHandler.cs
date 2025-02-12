using UnityEngine;

public class UnitMergeHandler : MonoBehaviour
{
    public void CheckForMerge()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider hitCollider in hitColliders)
        {
            Unit otherUnit = hitCollider.GetComponent<Unit>();
            if (otherUnit != null && otherUnit != GetComponent<Unit>() && CanMergeWith(otherUnit))
            {
                MergeWith(otherUnit);
                break;
            }
        }
    }

    bool CanMergeWith(Unit otherUnit)
    {
        Unit currentUnit = GetComponent<Unit>();
        return otherUnit.unitType == currentUnit.unitType && otherUnit.level == currentUnit.level;
    }

    void MergeWith(Unit otherUnit)
    {
        GetComponent<Unit>().LevelUp();
        Destroy(otherUnit.gameObject);
    }
}