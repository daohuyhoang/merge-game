using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private bool isBattleActive = false;
    private Unit unit;

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

    private void Start()
    {
        unit = GetComponent<Unit>();
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
    
    public void CheckVictory()
    {
        bool allEnemiesDead = EnemyManager.Instance.AreAllEnemiesDead();
        bool allUnitsInactive = ObjectPool.Instance.AreAllUnitsInactive();

        if (allEnemiesDead)
        {
            HandlePlayerVictory();
        }
        else if (allUnitsInactive)
        {
            HandleEnemyVictory();
        }
    }
    
    public void HandlePlayerVictory()
    {
        var winner = ObjectPool.Instance.GetFirstActiveUnit();
        if (winner != null)
        {
            winner.GetComponent<CheckVictory>().ShowPlayerVictory();
        }
        else
        {
            ResetCameraFOV();
            // PlayPlayerWinSound();
            SpinRewardSystem.Instance.ShowSpinPanel();
        }
    }

    public void HandleEnemyVictory()
    {
        var winner = EnemyManager.Instance.GetFirstActiveEnemy();
        if (winner != null)
        {
            winner.GetComponent<CheckVictory>().ShowEnemyVictory();
        }
        else
        {
            ResetCameraFOV();
            // PlayEnemyWinSound();
            SpinRewardSystem.Instance.ShowRewardOnDefeat();
        }
    }
}