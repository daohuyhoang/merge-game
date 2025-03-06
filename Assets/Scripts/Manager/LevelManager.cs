using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TMP_Text levelText;
    private int currentLevel = 1;

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
        UpdateLevelText();
    }

    public void IncreaseLevel()
    {
        currentLevel++;
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {currentLevel}";
        }
    }
}