using UnityEngine;

public class CheckVictory : MonoBehaviour
{
    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void ShowPlayerVictory()
    {
        BattleManager.Instance.ResetCameraFOV();
        FindUnitActive();
        BattleManager.Instance.PlayPlayerWinSound();
        ShowSpin();
    }

    private static void FindUnitActive()
    {
        Unit[] activeUnits = FindObjectsOfType<Unit>();

        foreach (Unit activeUnit in activeUnits)
        {
            if (activeUnit.UnitHealth.HP > 0)
            {
                activeUnit.VictoryAnimation();
            }
        }
    }

    public void ShowEnemyVictory()
    {
        BattleManager.Instance.ResetCameraFOV();
        FindUnitActive();
        BattleManager.Instance.PlayEnemyWinSound();
        ShowRewardOnDefeat();
    }
    
    private void ShowSpin()
    {
        if (SpinRewardSystem.Instance != null)
        {
            SpinRewardSystem.Instance.ShowSpinPanel();
            SpinRewardSystem.Instance.SetResultText("VICTORY!");
        }
        else
        {
            Debug.LogWarning("SpinRewardSystem is not assigned.");
        }
    }
    
    private void ShowRewardOnDefeat()
    {
        if (SpinRewardSystem.Instance != null)
        {
            SpinRewardSystem.Instance.ShowRewardOnDefeat();
        }
        else
        {
            Debug.LogWarning("SpinRewardSystem is not assigned.");
        }
    }
}