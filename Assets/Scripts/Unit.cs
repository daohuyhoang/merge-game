using UnityEngine;

public enum UnitType { Warrior, Archer }

public class Unit : MonoBehaviour
{
    public UnitType unitType;
    public int level = 1;
    public int hp = 100;
    public int damage = 10;

    public void LevelUp()
    {
        level++;
        hp += 50;
        Debug.Log($"{unitType} unit đã lên level {level} với {hp} HP và {damage} sát thương!");
    }
}