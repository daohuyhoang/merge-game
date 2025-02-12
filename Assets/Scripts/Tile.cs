using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool canSpawn = true;
    private UnitDragHandler currentUnit;

    public void SetUnit(UnitDragHandler unit)
    {
        currentUnit = unit;
    }

    public UnitDragHandler GetUnit()
    {
        return currentUnit;
    }
}
