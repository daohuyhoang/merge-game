using System;
using UnityEngine;
using System.Collections;

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
            isBattleActive = false;
            HandlePlayerVictory();
        }
        else if (allUnitsInactive)
        {
            isBattleActive = false;
            HandleEnemyVictory();
        }
    }
    
    public void HandlePlayerVictory()
    {
        ResetCameraFOV();
        PlayPlayerWinSound();

        Unit[] activeUnits = FindObjectsOfType<Unit>();
        foreach (Unit activeUnit in activeUnits)
        {
            if (activeUnit.UnitHealth.HP > 0)
            {
                activeUnit.VictoryAnimation();
            }
        }

        StartCoroutine(ShowSpinWithDelay());
    }

    private IEnumerator ShowSpinWithDelay()
    {
        yield return new WaitForSeconds(3f);

        if (SpinRewardSystem.Instance != null)
        {
            SpinRewardSystem.Instance.ShowSpinPanel();
            SpinRewardSystem.Instance.SetResultText("VICTORY!");
        }
        else
        {
            Debug.LogWarning("SpinRewardSystem.Instance is null!");
        }
    }

    public void HandleEnemyVictory()
    {
        ResetCameraFOV();
        PlayEnemyWinSound();

        Unit[] activeUnits = FindObjectsOfType<Unit>();
        foreach (Unit activeUnit in activeUnits)
        {
            if (activeUnit.UnitHealth.HP > 0)
            {
                activeUnit.VictoryAnimation();
            }
        }

        StartCoroutine(ShowRewardOnDefeatWithDelay());
    }

    private IEnumerator ShowRewardOnDefeatWithDelay()
    {
        yield return new WaitForSeconds(3f);

        if (SpinRewardSystem.Instance != null)
        {
            SpinRewardSystem.Instance.ShowRewardOnDefeat();
        }
        else
        {
            Debug.LogWarning("SpinRewardSystem.Instance is null!");
        }
    }
}