using UnityEngine;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private Button battleButton;
    [SerializeField] private Canvas buttonCanvas;

    [SerializeField] private AudioClip battleButtonSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (battleButton == null)
        {
            Debug.LogError("Battle Button is not assigned!");
            return;
        }

        battleButton.onClick.AddListener(OnBattleButtonClick);
    }

    private void OnBattleButtonClick()
    {
        PlayBattleButtonSound();

        StartBattle();
    }

    private void StartBattle()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SavePlayerUnits();
        }

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
        else
        {
            Debug.LogError("BattleManager instance is missing!");
        }

        if (buttonCanvas != null)
        {
            buttonCanvas.gameObject.SetActive(false);
        }
    }

    private void PlayBattleButtonSound()
    {
        if (audioSource != null && battleButtonSound != null)
        {
            audioSource.PlayOneShot(battleButtonSound);
        }
    }
}