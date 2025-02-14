using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public int HP { get; set; }
    public int Damage { get; set; }

    public void InitializeStats(int hp, int damage)
    {
        HP = hp;
        Damage = damage;
    }
}