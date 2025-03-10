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

    private float[] rewardAngles = { 0f, 45f, 90, 134.993f, 179.993f, 224.993f, 269.993f, 314.993f };
    private int[] multipliers = { 2, 3, 4, 5, 2, 3, 4, 5 };

    private int totalDamageDealt;
    private int rewardMultiplier;
    private bool isSpinning = false;
    private int reward;
    private bool isWin = false;

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
        spinWheel.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void AddToTotalDamageDealt(int damageDealt)
    {
        totalDamageDealt += damageDealt;
    }

    public void ShowSpinPanel()
    {
        spinPanel.SetActive(true);
    }

    public void ShowRewardOnDefeat()
    {
        spinPanel.SetActive(true);
        reward = totalDamageDealt;
        rewardText.text = $"YOU EARNED: +{reward}";
        resultText.text = "DEFEAT!";
        continueButton.gameObject.SetActive(true);
        spinButton.gameObject.SetActive(false);
        isWin = false;
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
            spinWheel.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator SpinWheel()
    {
        float spinDuration = 3f;
        float initialSpeed = 1080f;
        float currentAngle = 0f;

        int randomIndex = Random.Range(0, rewardAngles.Length);
        float targetAngle = -rewardAngles[randomIndex] + 360f * Random.Range(3, 6);
        float finalDisplayAngle = -rewardAngles[randomIndex];

        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spinDuration;
            float speed = initialSpeed * (1f - t * t);
            float angleDelta = speed * Time.deltaTime;
            currentAngle += angleDelta;

            spinWheel.rotation = Quaternion.Euler(0, 0, currentAngle);

            if (currentAngle >= targetAngle - 360f)
            {
                float remainingAngle = targetAngle - currentAngle;
                if (remainingAngle > 0)
                {
                    angleDelta = Mathf.Min(angleDelta, remainingAngle);
                    currentAngle += angleDelta;
                    spinWheel.rotation = Quaternion.Euler(0, 0, currentAngle);
                }
            }

            yield return null;
        }

        spinWheel.rotation = Quaternion.Euler(0, 0, finalDisplayAngle);
        rewardMultiplier = multipliers[randomIndex];
        Debug.Log($"Multiplier: {rewardMultiplier}");
        ShowReward();
    }


    private void ShowReward()
    {
        reward = totalDamageDealt * rewardMultiplier;
        rewardText.text = $"YOU EARNED: +{reward}";
        continueButton.gameObject.SetActive(true);
        isSpinning = false;
        isWin = true;
    }

    private void OnContinueButtonClicked()
    {
        CoinManager.Instance.AddCoin(reward);

        if (isWin)
        {
            LoadNextScene();
        }
        else
        {
            RestartCurrentScene();
        }

        HideSpinPanel();
    }

    public void HideSpinPanel()
    {
        spinPanel.SetActive(false);
        continueButton.gameObject.SetActive(false);
        spinButton.interactable = true;
        spinButton.gameObject.SetActive(true);
        totalDamageDealt = 0;
    }

    private void LoadNextScene()
    {
        LevelManager.Instance.IncreaseLevel();

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

    private void RestartCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}