using UnityEngine;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private Button battleButton;
    [SerializeField] private Canvas buttonCanvas;

    private void Start()
    {
        if (battleButton == null)
        {
            Debug.LogError("Battle Button is not assigned!");
            return;
        }

        battleButton.onClick.AddListener(StartBattle);
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
}