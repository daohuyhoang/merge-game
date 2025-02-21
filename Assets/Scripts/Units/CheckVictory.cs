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
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}