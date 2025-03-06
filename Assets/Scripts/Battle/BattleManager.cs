using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private bool isBattleActive = false;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private AudioClip playerWinSound;
    [SerializeField] private AudioClip enemyWinSound;
    
    private AudioSource audioSource;
    private bool hasPlayerWon = false;
    private bool hasEnemyWon = false;

    public bool IsBattleActive() => isBattleActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void StartBattle()
    {
        if (!isBattleActive)
        {
            isBattleActive = true;

            if (mainCamera != null)
            {
                mainCamera.fieldOfView = 56f;
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

    public void PlayPlayerWinSound()
    {
        if (!hasPlayerWon && audioSource != null && playerWinSound != null)
        {
            audioSource.PlayOneShot(playerWinSound);
            hasPlayerWon = true;
        }
    }

    public void PlayEnemyWinSound()
    {
        if (!hasEnemyWon && audioSource != null && enemyWinSound != null)
        {
            audioSource.PlayOneShot(enemyWinSound);
            hasEnemyWon = true;
        }
    }
}