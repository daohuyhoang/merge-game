using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool canSpawn = true;
    private UnitDragHandler currentUnit;

    public void SetUnit(UnitDragHandler unit)
    {
        currentUnit = unit;
        canSpawn = unit == null;
    }

    public UnitDragHandler GetUnit() => currentUnit;
}