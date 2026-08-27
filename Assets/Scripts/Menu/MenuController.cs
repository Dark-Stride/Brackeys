using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MenuController : MonoBehaviour
{
   //Audio Seettings
    [Header("Master Volume")]
    [SerializeField] private TMP_Text volumeValue = null;      // Displays the current master volume value.
    [SerializeField] private Slider volumeSlider = null;       // Controls the overall game volume.
    [SerializeField] private float defaultVolume = 1.0f;       // Default master volume.

    [Header("Music")]
    [SerializeField] private Slider musicSlider;               // Controls the background music volume.
    [SerializeField] private TMP_Text musicValue;              // Displays the current music volume.
   
    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;                 // Controls sound effect volume.
    [SerializeField] private TMP_Text sfxValue;                // Displays the current SFX volume.

    // Stores the SFX volume.
    private float sfxVolume = 1f;

    // UI REFERENCES
    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null; // "Settings Saved" popup.

    [Header("Levels To Load")]
    public string newGameLevel;                                   // Scene loaded when starting a new game.
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialogue = null;

    private void Start()
    {
        LoadAudioSettings();
    }

    /// Loads all saved audio settings when the game starts.
    private void LoadAudioSettings()
    {
            // Master Volume 
            float masterVolume = PlayerPrefs.GetFloat("MasterVolume", defaultVolume);

            AudioListener.volume = masterVolume;
            volumeSlider.value = masterVolume;
            volumeValue.text = masterVolume.ToString("0.0");

            // Music Volume 
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);

            AudioManager.Instance.SetMusicVolume(musicVolume);
            musicSlider.value = musicVolume;
            musicValue.text = musicVolume.ToString("0.0");

            // SFX Volume 
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);

            sfxSlider.value = sfxVolume;
            sfxValue.text = sfxVolume.ToString("0.0");
    }

    // SCENE MANAGEMENT
    // Starts a brand new game.
    public void NewGameDialogueYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    // Loads the player's last saved level.
    public void LoadGameDialogueYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialogue.SetActive(true);
        }
    }

    // Exits the application.
    public void ExitButton()
    {
        Application.Quit();
    }

    // MASTER VOLUME
    // Called whenever the Master Volume slider changes.
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValue.text = volume.ToString("0.0");
    }

    // MUSIC
    // Called whenever the Music slider changes.
    public void SetMusic(float volume)
    {
        // Tell the AudioManager to change the music volume.
        AudioManager.Instance.SetMusicVolume(volume);

        // Update the percentage shown on screen.
        musicValue.text = volume.ToString("0.0");
    }

    // SOUND EFFECTS
    // Called whenever the SFX slider changes.
    public void SetSFX(float volume)
    {
        sfxVolume = volume;
        sfxValue.text = volume.ToString("0.0");

        // Update the AudioManager's SFX volume.
        AudioManager.Instance.SetSFXVolume(volume);

        // Play a preview sound.
        AudioManager.Instance.PlaySlider();
    }

    /// <summary>
    /// Saves all audio settings at once.
    /// Called by the Apply button.
    /// </summary>
    public void ApplyAudioSettings()
    {
        // Save Master Volume
        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);

        // Save Music Volume
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        // Save SFX Volume
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        // Write everything to disk
        PlayerPrefs.Save();

        // Play a UI click sound.
        AudioManager.Instance.PlayClick();

        // Show the confirmation popup.
        StartCoroutine(ConfirmationBox());
    }

    // Resets all audio settings back to their default values.
    public void ResetButton(string menuType)
    {
        // Save the reset values.
        ApplyAudioSettings();
    }

    // CONFIRMATION POPUP
    // Displays a confirmation message for two seconds after saving settings.
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2f);
        confirmationPrompt.SetActive(false);
    }
}