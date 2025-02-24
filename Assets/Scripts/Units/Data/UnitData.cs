using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit/UnitData")]
public class UnitData : ScriptableObject
{
    public Unit.UnitTypeEnum unitType;
    public int maxLevel;
    public int[] hpByLevel;
    public int[] atkByLevel;
}