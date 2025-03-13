using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private Image buttonImage;

    private void Start()
    {
        if (soundButton == null)
        {
            Debug.LogError("Sound Button is not assigned!");
            return;
        }

        buttonImage = soundButton.GetComponent<Image>();
        UpdateButtonImage();

        soundButton.onClick.AddListener(OnSoundButtonClick);
    }

    private void OnSoundButtonClick()
    {
        SoundManager.Instance.ToggleSound();
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (SoundManager.Instance.IsSoundOn())
        {
            buttonImage.sprite = soundOnSprite;
        }
        else
        {
            buttonImage.sprite = soundOffSprite;
        }
    }
}