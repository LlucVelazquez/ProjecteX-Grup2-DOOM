using UnityEngine;

public enum SFXType
{

}

public enum MusicType
{
    
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    private void Reset()
    {
        int sourcesCount = GetComponents<AudioSource>().Length;
        for (int i = 0; i < 2 - sourcesCount; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

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

        _musicSource = GetComponents<AudioSource>()[0];
        _sfxSource = GetComponents<AudioSource>()[1];
    }

    public void PlayeMusic(MusicType music, float volume = 1f)
    {

    }

    public void PlaySFX(SFXType sfx, float volume = 1f)
    {

    }
}
