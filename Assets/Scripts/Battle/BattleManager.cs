using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    
    private bool isBattleActive = false;

    [SerializeField] private Camera mainCamera;

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

            if (mainCamera != null)
            {
                mainCamera.fieldOfView = 47.6f;
            }

            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach (Unit unit in allUnits)
            {
                unit.FindTarget();
            }
        }
    }

    public void ResetCameraFOV()
    {
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = 60f;
        }
    }
}