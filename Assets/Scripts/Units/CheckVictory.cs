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
            SpinRewardSystem.Instance.ShowSpinPanel();
            SpinRewardSystem.Instance.SetResultText("Victory!");
        }
    }

    private void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}