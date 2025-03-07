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
        unit.VictoryAnimation();
        BattleManager.Instance.PlayPlayerWinSound();
        ShowSpin();
    }
    
    public void ShowEnemyVictory()
    {
        BattleManager.Instance.ResetCameraFOV();
        unit.VictoryAnimation();
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