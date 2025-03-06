using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private bool isSoundOn = true;

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

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        ApplySoundSettings();
    }

    private void ApplySoundSettings()
    {
        AudioListener.volume = isSoundOn ? 1 : 0;
    }

    public bool IsSoundOn()
    {
        return isSoundOn;
    }
}