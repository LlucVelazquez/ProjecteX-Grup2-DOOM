using System;
using UnityEngine;

public enum SoundType
{
    PlayerFootsteps,
    PlayerSprintFootsteps,
    PlayerHurt,
    PlayerDeath,
    ShotgunFire,
    ShotgunReload,
    SwordEquip,
    SwordAirSlash,
    SwordSolidHit,
    SwordEnemyHit,
    BarrelExplosion,
}

public enum MusicType
{
    MainMusic,
    LevelMusic
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume Keys")]
    [SerializeField] private string _masterVolumeKey = "MasterVolume";
    [SerializeField] private string _musicVolumeKey = "MusicVolume";
    [SerializeField] private string _sfxVolumeKey = "SfXVolume";

    [Header("Deafult Volumes")]
    [SerializeField] private float _defaultMasterVolume = 1f;
    [SerializeField] private float _defaultMusicVolume = 1f;
    [SerializeField] private float _defaultSFXVolume = 1f;

    [Header("Audio Clips")]
    [SerializeField] private MusicList[] _musics = new MusicList[0];
    [SerializeField] private SoundList[] _sounds = new SoundList[0];

    private AudioSource _musicSource;
    private AudioSource _soundSource;

    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;

    private void OnEnable()
    {
        OptionsMenuManager.OnOptionsChange += UpdateVolumes;

#if UNITY_EDITOR
        string[] soundNames = Enum.GetNames(typeof(SoundType));
        string[] musicNames = Enum.GetNames(typeof(MusicType));

        Array.Resize(ref _sounds, soundNames.Length);
        Array.Resize(ref _musics, musicNames.Length);

        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].name = soundNames[i];
        }
        for (int i = 0; i < _musics.Length; i++)
        {
            _musics[i].name = musicNames[i];
        }
#endif
    }

    private void OnDisable()
    {
        OptionsMenuManager.OnOptionsChange -= UpdateVolumes;
    }

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
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

        _musicSource = GetComponents<AudioSource>()[0];
        _soundSource = GetComponents<AudioSource>()[1];

        _musicSource.loop = true;
    }

    private void Start()
    {
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        _masterVolume = PlayerPrefs.GetFloat(_masterVolumeKey, _defaultMasterVolume);
        _musicVolume = PlayerPrefs.GetFloat(_musicVolumeKey, _defaultMusicVolume);
        _sfxVolume = PlayerPrefs.GetFloat(_sfxVolumeKey, _defaultSFXVolume);

        _musicSource.volume = _musicVolume * _masterVolume;
        _soundSource.volume = _sfxVolume * _masterVolume;
    }

    public void PlaySound(SoundType sound)
    {
        PlaySound(sound, _sfxVolume * _masterVolume);
    }

    public void PlaySound(SoundType sound, float volume)
    {
        AudioClip[] clips = _sounds[(int)sound].SoundClips;
        AudioClip rclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        _soundSource.PlayOneShot(rclip, volume);
    }

    public void PlayeMusic(MusicType music)
    {
        PlayeMusic(music, _musicVolume * _masterVolume);
    }

    public void PlayeMusic(MusicType music, float volume)
    {
        _musicSource.clip = _musics[(int)music].MusicClip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] _soundClips;

    public AudioClip[] SoundClips => _soundClips;
}

[Serializable]
public struct  MusicList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip _musicClip;

    public AudioClip MusicClip => _musicClip;
}