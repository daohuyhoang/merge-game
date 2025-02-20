using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private UnitHealth unitHealth;
    [SerializeField] private Image fillImage;

    [Header("Health Bar Colors")]
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color enemyColor = Color.red;

    private void Awake()
    {
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        if (fillImage == null && healthSlider != null)
        {
            fillImage = healthSlider.fillRect.GetComponent<Image>();
        }

        if (unitHealth == null)
        {
            unitHealth = GetComponentInParent<UnitHealth>();
        }

        if (healthSlider == null || fillImage == null || unitHealth == null)
        {
            Debug.LogError("HealthSlider, FillImage, or UnitHealth is missing!");
            enabled = false;
            return;
        }

        UpdateHealthBar();
        SetHealthBarColor();
    }

    private void OnEnable()
    {
        unitHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        unitHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        if (unitHealth != null && healthSlider != null)
        {
            float healthPercentage = (float)unitHealth.HP / unitHealth.MaxHP;
            healthSlider.value = healthPercentage;
        }
    }

    public void SetHealthBarColor()
    {
        if (fillImage == null) return;

        if (unitHealth.CompareTag("Player"))
        {
            fillImage.color = playerColor;
        }
        else if (unitHealth.CompareTag("Enemy"))
        {
            fillImage.color = enemyColor;
        }
    }
}