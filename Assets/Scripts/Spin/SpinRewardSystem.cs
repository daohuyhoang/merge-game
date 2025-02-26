using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpinRewardSystem : MonoBehaviour
{
    [SerializeField] private GameObject spinPanel;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Transform spinWheel;

    private float[] rewardAngles = { 0f, 179.993f, 45f, 224.993f, 90f, 269.993f, 134.993f, 314.993f };
    private int[] multipliers = { 2, 2, 3, 3, 4, 4, 5, 5 };

    private int totalDamageDealt;
    private int rewardMultiplier;
    private bool isSpinning = false;
    private int reward;

    public static SpinRewardSystem Instance { get; private set; }

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

    private void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        continueButton.gameObject.SetActive(false);
    }

    public void AddToTotalDamageDealt(int damageDealt)
    {
        totalDamageDealt += damageDealt;
    }

    public void ShowSpinPanel()
    {
        spinPanel.SetActive(true);
    }
    
    public void SetResultText(string text)
    {
        resultText.text = text;
    }

    private void StartSpin()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            spinButton.interactable = false;
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator SpinWheel()
    {
        float spinDuration = 3f;
        float elapsedTime = 0f;

        int randomIndex = Random.Range(0, multipliers.Length);
        rewardMultiplier = multipliers[randomIndex];
        float targetAngle = rewardAngles[randomIndex] + 360 * 4;

        while (elapsedTime < spinDuration)
        {
            float zRotation = Mathf.Lerp(0, targetAngle, elapsedTime / spinDuration);
            spinWheel.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spinWheel.rotation = Quaternion.Euler(0, 0, targetAngle % 360);
        ShowReward();
    }

    private void ShowReward()
    {
        reward = totalDamageDealt * rewardMultiplier;

        rewardText.text = $"YOU EARNED: +{reward}";

        continueButton.gameObject.SetActive(true);

        isSpinning = false;
    }

    private void OnContinueButtonClicked()
    {
        CoinManager.Instance.AddCoin(reward);
        SaveAndLoadNextScene();
        HideSpinPanel();
    }

    public void HideSpinPanel()
    {
        spinPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        spinButton.interactable = true;
        totalDamageDealt = 0;
    }
    
    private void SaveAndLoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
        CoinManager.Instance.UpdateCoinUI();
    }
}