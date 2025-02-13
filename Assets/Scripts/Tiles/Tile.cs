using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool canSpawn = true;
    private Unit currentUnit;

    public bool CanSpawn
    {
        get => canSpawn;
        set => canSpawn = value;
    }

    public void SetUnit(Unit unit)
    {
        currentUnit = unit;
        canSpawn = unit == null;
    }

    public Unit GetUnit() => currentUnit;
}