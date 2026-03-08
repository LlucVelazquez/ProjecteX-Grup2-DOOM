using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class ProjectileBehaviour : MonoBehaviour, IResettable
{
    [HideInInspector] public ProjectileFiringBehaviour shooter;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float speed;
    [HideInInspector] public int damageMulti;
    [HideInInspector] public int maxBaseDamage;

    private Rigidbody _rigidbody;
    private AudioSource _source;

    private void OnEnable() => OptionsMenuManager.OnOptionsChange += UpdateVolume;
    private void OnDisable() => OptionsMenuManager.OnOptionsChange -= UpdateVolume;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
        UpdateVolume();
        PlaySound();
    }

    private void UpdateVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MasterVolumeKey, OptionSettingsUtils.DefaultMasterVolume);
        float sfxVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.SFXVolumeKey, OptionSettingsUtils.DefaultSFXVolume);

        _source.volume = sfxVolume * masterVolume * 0.5f;
    }

    private void PlaySound()
    {
        _source.loop = true;
        _source.clip = AudioManager.Instance.GetSound(SoundType.Projectile);
        _source.Play();
    }

    private void Update()
    {
        _rigidbody.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != shooter.gameObject.layer && other.gameObject.layer != gameObject.layer)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(shooter.targetLayerName))
            {
                if (other.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
                {
                    target.TakeDamage(DamageUtils.GetDamageByMulti(maxBaseDamage, damageMulti));
                }
                else
                {
                    Debug.LogWarning($"The object '{other.gameObject.name}' on layer '{shooter.targetLayerName}' does not implement ITargeteable. AttackBehaviour will not function properly.");
                }
            }
            AudioManager.Instance.PlaySoundAtPoint(SoundType.ProjectileHit, transform.position);
            ReturnToShooter();
        }
    }

    private void ReturnToShooter()
    {
        gameObject.SetActive(false);
        shooter.ProjectileStackPush(gameObject);
    }

    public void OnReset(bool fullRestart)
    {
        ReturnToShooter();
    }
}
