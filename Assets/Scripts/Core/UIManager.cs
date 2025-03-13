using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject victoryPanel;
    public Button poolButton;

    private void Awake()
    {
        Instance = this;
        victoryPanel.SetActive(false);
        poolButton.onClick.AddListener(OnPoolButtonClicked);
    }

    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
    }

    private void OnPoolButtonClicked()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        foreach (Unit unit in allUnits)
        {
            ObjectPool.Instance.ReturnToPool(unit.UnitType , unit.gameObject);
        }
        victoryPanel.SetActive(false);
    }
}