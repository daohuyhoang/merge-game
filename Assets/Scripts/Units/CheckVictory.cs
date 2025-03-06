using System.Collections;
using UnityEngine;

public class CheckVictory : MonoBehaviour
{
    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void CheckForEnemyVictory()
    {
        if (ObjectPool.Instance.AreAllUnitsInactive())
        {
            BattleManager.Instance.ResetCameraFOV();
            unit.VictoryAnimation();
            BattleManager.Instance.PlayEnemyWinSound();
            StartCoroutine(ShowRewardOnDefeat());
        }
    }

    public void CheckForPlayerVictory()
    {
        if (EnemyManager.Instance.AreAllEnemiesDead())
        {
            BattleManager.Instance.ResetCameraFOV();
            unit.VictoryAnimation();
            BattleManager.Instance.PlayPlayerWinSound();
            StartCoroutine(ShowSpinAfterDelay(1f));
        }
    }

    private IEnumerator ShowSpinAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpinRewardSystem.Instance.ShowSpinPanel();
        SpinRewardSystem.Instance.SetResultText("VICTORY!");
    }

    private IEnumerator ShowRewardOnDefeat()
    {
        yield return new WaitForSeconds(1f);
        SpinRewardSystem.Instance.ShowRewardOnDefeat();
    }
}