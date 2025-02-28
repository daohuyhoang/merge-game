using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            RestartCurrentScene();
        }
    }

    public void CheckForPlayerVictory()
    {
        if (EnemyManager.Instance.AreAllEnemiesDead())
        {
            BattleManager.Instance.ResetCameraFOV();
            unit.VictoryAnimation();
            StartCoroutine(ShowSpinAfterDelay(1f));
        }
    }

    private IEnumerator ShowSpinAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpinRewardSystem.Instance.ShowSpinPanel();
        SpinRewardSystem.Instance.SetResultText("Victory!");
    }

    private void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}