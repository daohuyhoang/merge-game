using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        foreach (Unit unit in allUnits)
        {
            unit.FindTarget();
        }
    }
}