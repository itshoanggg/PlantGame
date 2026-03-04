using UnityEngine;

/// <summary>
/// Audio Manager - Quản lý tất cả âm thanh trong game
/// Background music và sound effects
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Background Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Sound Effects")]
    public AudioClip buttonClickSound;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    [Header("Audio Sources")]
    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        // Singleton pattern - chỉ có 1 AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không bị destroy khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tạo 2 AudioSource: 1 cho music, 1 cho SFX
        SetupAudioSources();
    }

    void SetupAudioSources()
    {
        // Music source (looping)
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        // SFX source (one-shot)
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }

    // ========================================
    // MUSIC FUNCTIONS
    // ========================================

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // Đã đang chơi nhạc này rồi

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    // ========================================
    // SOUND EFFECTS FUNCTIONS
    // ========================================

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    // ========================================
    // UTILITY
    // ========================================

    public void MuteAll(bool mute)
    {
        musicSource.mute = mute;
        sfxSource.mute = mute;
    }
}
