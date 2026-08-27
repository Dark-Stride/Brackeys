using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Global reference to the AudioManager.
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Plays sound effects such as punches, UI clicks and footsteps.")]
    [SerializeField] private AudioSource sfxSource;

    [Tooltip("Plays background music.")]
    [SerializeField] private AudioSource musicSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip sliderClip;

    // Current SFX volume.
    private float sfxVolume = 1f;

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

    // Plays a one-shot sound effect.
    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
            return;

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, volume);
    }

    // UI Sounds
    public void PlayHover()
    {
        PlaySFX(hoverClip, sfxVolume);
    }

    public void PlayClick()
    {
        PlaySFX(clickClip, sfxVolume);
    }

    public void PlaySlider()
    {
        PlaySFX(sliderClip, sfxVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    // Music
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}