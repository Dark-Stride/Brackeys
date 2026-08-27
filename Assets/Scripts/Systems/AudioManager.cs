using UnityEngine;

namespace Scripts.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        void Awake() => Instance = this;

        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip, volume);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}
