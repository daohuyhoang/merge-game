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

    [SerializeField] private float stopPower = 100f;

    private Rigidbody2D rbody;
    private int inRotate = 0;
    private float t;
    private float rotatePower;

    private int[] multipliers = { 2, 5, 4, 3, 2, 5, 4, 3 };
    private int totalDamageDealt;
    private int rewardMultiplier;
    private bool isSpinning = false;
    private int reward;
    private bool isWin = false;
    private bool hasSpun = false;

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
        rbody = spinWheel.GetComponent<Rigidbody2D>();
        if (rbody == null)
        {
            Debug.LogError("SpinWheel cần có Rigidbody2D!");
        }

        spinButton.onClick.AddListener(StartSpin);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        continueButton.gameObject.SetActive(false);
        spinWheel.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        if (rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= stopPower * Time.deltaTime;
            rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
        }

        if (rbody.angularVelocity == 0 && inRotate == 1)
        {
            t += Time.deltaTime;
            if (t >= 0.5f)
            {
                GetReward();
                inRotate = 0;
                t = 0;
                isSpinning = false;
                ShowReward();
            }
        }
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
        if (!isSpinning && !hasSpun)
        {
            isSpinning = true;
            inRotate = 1;
            spinButton.interactable = false;
            spinWheel.rotation = Quaternion.Euler(0, 0, 0);
            rbody.angularVelocity = 0;
            rotatePower = Random.Range(400f, 600f);
            rbody.AddTorque(rotatePower);
            hasSpun = true;
            Debug.Log("Rotate Power: " + rotatePower);
        }
    }

    private void GetReward()
    {
        float rot = spinWheel.eulerAngles.z;
        float normalizedRot = (rot + 360f) % 360f;
        float adjustedRot = (normalizedRot + 22.5f) % 360f;

        int index = Mathf.FloorToInt(adjustedRot / 45f) % multipliers.Length;
        rewardMultiplier = multipliers[index];

        Debug.Log($"Final Angle: {normalizedRot}, Multiplier: {rewardMultiplier}");
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
        spinButton.interactable = false;
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