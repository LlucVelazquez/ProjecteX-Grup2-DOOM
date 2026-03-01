using System.Collections;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    [Header("Genral Settings")]
    [SerializeField] private float _effectsDamageInterval = 1f;
    [SerializeField] private float _effectsDuration = 0.3f;

    [Header("Damage Effect")]
    [SerializeField] private string _damageEffectVignetteRadiusName = "VignetteRadius";
    [SerializeField] private string _damageEffectBlurMultiplierName = "BlurMultiplier";
    [SerializeField] private string _damageEffectBlurTintName = "BlurTint";
    [SerializeField] private Color _damageEffectTint;

    private Coroutine _damageEffectsCoroutine;
    private bool _bloodEffectEnabled = false;
    private float _timer;

    private void OnEnable() => PlayerHealth.OnLowHealth += ShowDamageEffects;
    private void OnDisable() => PlayerHealth.OnLowHealth -= ShowDamageEffects;

    private void Awake()
    {
        Shader.SetGlobalFloat(_damageEffectVignetteRadiusName, 1f);
        Shader.SetGlobalFloat(_damageEffectBlurMultiplierName, 0f);
        Shader.SetGlobalColor(_damageEffectBlurTintName, Color.white);
    }

    private void Update()
    {
        if (_bloodEffectEnabled && Time.time > _timer)
        {
            TriggerDamageEffects(Random.Range(.1f, 1f));
            _timer = Time.time + _effectsDamageInterval;
        }
    }

    private void ShowDamageEffects(bool show)
    {
        _bloodEffectEnabled = show;

        if (!show)
        {
            if (_damageEffectsCoroutine != null) StopCoroutine(_damageEffectsCoroutine);
            Shader.SetGlobalFloat(_damageEffectVignetteRadiusName, 1f);
            Shader.SetGlobalFloat(_damageEffectBlurMultiplierName, 0f);
            Shader.SetGlobalColor(_damageEffectBlurTintName, Color.white);
        }
    }

    public void TriggerDamageEffects(float intensity)
    {
        if (_damageEffectsCoroutine != null) StopCoroutine(_damageEffectsCoroutine);
        _damageEffectsCoroutine = StartCoroutine(DamageEffects(intensity));
    }

    private IEnumerator DamageEffects(float intensity)
    {
        float targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
        float targetBlur = Remap(intensity, 0, 1, 0.5f, 1f);
        float currentRadius = 1f;

        for (float t = 0; t < 1f; t += Time.deltaTime / _effectsDuration)
        {
            currentRadius = Mathf.Lerp(1f, targetRadius, t);
            Shader.SetGlobalFloat(_damageEffectVignetteRadiusName, currentRadius);
            Shader.SetGlobalFloat(_damageEffectBlurMultiplierName, Mathf.Lerp(0f, targetBlur, t));
            Shader.SetGlobalColor(_damageEffectBlurTintName, Color.Lerp(Color.white, _damageEffectTint, t));
            yield return null;
        }

        for (float t = 0; t < 1f; t += Time.deltaTime / _effectsDuration)
        {
            currentRadius = Mathf.Lerp(targetRadius, 1f, t);
            Shader.SetGlobalFloat(_damageEffectVignetteRadiusName, currentRadius);
            Shader.SetGlobalFloat(_damageEffectBlurMultiplierName, Mathf.Lerp(targetBlur, 0f, t));
            Shader.SetGlobalColor(_damageEffectBlurTintName, Color.Lerp(_damageEffectTint, Color.white, t));
            yield return null;
        }

        Shader.SetGlobalFloat(_damageEffectVignetteRadiusName, currentRadius);
        Shader.SetGlobalFloat(_damageEffectBlurMultiplierName, 0f);
        Shader.SetGlobalColor(_damageEffectBlurTintName, Color.white);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        => Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
}
