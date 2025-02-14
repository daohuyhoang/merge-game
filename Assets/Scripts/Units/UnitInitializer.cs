using UnityEngine;

public class UnitInitializer : MonoBehaviour
{
    [SerializeField] private int baseHP = 100;
    [SerializeField] private int baseDamage = 10;

    private void Start()
    {
        UnitStats stats = GetComponent<UnitStats>();
        if (stats != null)
        {
            stats.InitializeStats(baseHP, baseDamage);
        }
    }
}