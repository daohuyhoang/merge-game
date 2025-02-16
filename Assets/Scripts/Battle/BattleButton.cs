using UnityEngine;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour
{
    [SerializeField] private Button battleButton;

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
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
        else
        {
            Debug.LogError("BattleManager instance is missing!");
        }
    }
}