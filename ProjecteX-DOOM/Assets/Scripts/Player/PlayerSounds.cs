using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    [Header("Footsetps")]
    [SerializeField] private float _walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;

    private AudioSource _source;

    private float _footstepTimer;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerHurt += PlayHurtSound;
        PlayerHealth.OnLowHealth += PlayLowHealthSound;
        PlayerHealth.OnPlayerDeath += PlayDeathSound;

        OptionsMenuManager.OnOptionsChange += UpdateVolume;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= PlayHurtSound;
        PlayerHealth.OnLowHealth -= PlayLowHealthSound;
        PlayerHealth.OnPlayerDeath -= PlayDeathSound;

        OptionsMenuManager.OnOptionsChange -= UpdateVolume;
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MasterVolumeKey, OptionSettingsUtils.DefaultMasterVolume);
        float sfxVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.SFXVolumeKey, OptionSettingsUtils.DefaultSFXVolume);

        _source.volume = sfxVolume * masterVolume;
    }

    public void PlayPlayerFootsteps(bool isGrounded, bool isMoving, bool isSprinting)
    {
        if (!isGrounded || isMoving)
        {
            _footstepTimer = 0f;
            return;
        }

        float stepInterval = isSprinting ? sprintStepInterval : _walkStepInterval;

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0f)
        {
            if (!isSprinting)
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerFootsteps, 0.2f);
            }
            else
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerSprintFootsteps, 0.2f);
            }
            _footstepTimer = stepInterval;
        }
    }

    private void PlayHurtSound()
    {
        AudioManager.Instance.PlaySound(SoundType.PlayerHurt);
    }

    private void PlayLowHealthSound(bool play)
    {
        if (play)
        {
            AudioManager.Instance.PlaySound(SoundType.PlayerLowHealth);

            _source.loop = true;
            _source.clip = AudioManager.Instance.GetSound(SoundType.PlayerHighHeartbeat);
            _source.Play();
        }
        else
        {
            _source.loop = false;
            _source.clip = null;
            _source.Stop();
        }
    }

    private void PlayDeathSound()
    {
        AudioManager.Instance.PlaySound(SoundType.PlayerDeath);

        _source.loop = false;
        _source.clip = null;
        _source.Stop();
    }
}
