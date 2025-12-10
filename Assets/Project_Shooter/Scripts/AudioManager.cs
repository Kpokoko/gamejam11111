using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource MusicSource;

    [Header("SFX")]
    public AudioSource SFXSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Воспроизведение музыки
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (MusicSource.clip == clip) return;

        MusicSource.clip = clip;
        MusicSource.loop = loop;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    // Воспроизведение звуков
    public void PlaySFX(AudioClip clip, float volume = 0.5f)
    {
        SFXSource.PlayOneShot(clip, volume);
    }
}