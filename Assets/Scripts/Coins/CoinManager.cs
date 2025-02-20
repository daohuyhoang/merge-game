using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int totalCoin = 120;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int damageDealt)
    {
        totalCoin += damageDealt;
        Debug.Log($"Added {damageDealt} coins. Total coins: {totalCoin}");
    }

    public int GetTotalCoin()
    {
        return totalCoin;
    }
}