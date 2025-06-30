using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private MusicManager musicManager;

    private TextMeshProUGUI soundVolumeText;
    private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        soundVolumeText = transform.Find("SoundVolumeText").GetComponent<TextMeshProUGUI>();

        musicVolumeText = transform.Find("MusicVolumeText").GetComponent<TextMeshProUGUI>();

        transform
            .Find("SoundUp")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                soundManager.IncreaseVolume();
                UpdateSoundVolumeText();
            });

        transform
            .Find("SoundDown")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                soundManager.DecreaseVolume();
                UpdateSoundVolumeText();
            });

        transform
            .Find("MusicUp")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                musicManager.IncreaseVolume();
                UpdateMusicVolumeText();
            });

        transform
            .Find("MusicDown")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                musicManager.DecreaseVolume();
                UpdateMusicVolumeText();
            });

        transform
            .Find("MainMenuBtn")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
            Time.timeScale = 1f;
                GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
            });
    }

    private void Start()
    {
        UpdateSoundVolumeText();
        gameObject.SetActive(false);
    }

    private void UpdateSoundVolumeText()
    {
        soundVolumeText.SetText((soundManager.GetVolume() * 10).ToString("F0"));
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.SetText((musicManager.GetVolume() * 10).ToString("F0"));
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        // Pause the game by make update = 0
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
