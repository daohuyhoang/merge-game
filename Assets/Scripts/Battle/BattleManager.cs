using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    
    private bool isBattleActive = false;
    
    public bool IsBattleActive() => isBattleActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBattle()
    {
        if (!isBattleActive)
        {
            isBattleActive = true;
            Unit[] allUnits = FindObjectsOfType<Unit>();
            
            foreach (Unit unit in allUnits)
            {
                unit.FindTarget();
            }
        }
    }
}