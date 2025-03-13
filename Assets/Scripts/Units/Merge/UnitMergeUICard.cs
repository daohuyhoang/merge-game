using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitMergeUICard : MonoBehaviour
{
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        continueButton.onClick.AddListener(Hide);
    }

    public void DisplayUnitInfo(Sprite unitSprite, int hp, int damage, string unitType, int unitLevel)
    {
        unitImage.sprite = unitSprite;
        hpText.text = $"{hp}";
        damageText.text = $"{damage}";
        text.text = $"{unitType} Level {unitLevel}";
        
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}