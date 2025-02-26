using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    
    public static CoinManager Instance;

    private int totalCoin = 120;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoin(int amount)
    {
        totalCoin += amount;
        Debug.Log($"Added {amount} coins. Total coins: {totalCoin}");
        UpdateCoinUI();
    }

    public int GetTotalCoin()
    {
        return totalCoin;
    }
    
    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"{totalCoin}";
        }
    }
}