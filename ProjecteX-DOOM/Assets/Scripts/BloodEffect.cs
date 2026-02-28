using System.Collections;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private Material _bloodEffectMat;
    [SerializeField] private string _bloodEffectVignetteRadiusName = "VignetteRadius";
    [SerializeField] private float _effectInterval = 1f;

    private Coroutine _bloodEffectCoroutine;
    private bool _bloodEffectEnabled = false;
    private float _timer;

    private void OnEnable() => PlayerHealth.OnLowHealth += ShowEffect;
    private void OnDisable() => PlayerHealth.OnLowHealth -= ShowEffect;

    private void Update()
    {
        if (_bloodEffectEnabled)
        {
            if (Time.time > _timer)
            {
                ScreenBloodEffect(Random.Range(.1f, 1f));
                _timer = Time.time + _effectInterval;
            }
        }
    }

    private void ShowEffect(bool show)
    {
        _bloodEffectEnabled = show;
    }

    private void ScreenBloodEffect(float intensity)
    {
        if (_bloodEffectCoroutine != null) StopCoroutine(_bloodEffectCoroutine);
        _bloodEffectCoroutine = StartCoroutine(BooldEffect(intensity));
    }

    private IEnumerator BooldEffect(float intensity)
    {
        float targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
        float currentRadius = 1f;
        float duration = 0.3f;

        // Fade in (cap al target)
        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            currentRadius = Mathf.Lerp(1f, targetRadius, t);
            Shader.SetGlobalFloat(_bloodEffectVignetteRadiusName, currentRadius);
            yield return null;
        }

        // Fade out (torna a 1)
        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            currentRadius = Mathf.Lerp(targetRadius, 1f, t);
            Shader.SetGlobalFloat(_bloodEffectVignetteRadiusName, currentRadius);
            yield return null;
        }

        Shader.SetGlobalFloat(_bloodEffectVignetteRadiusName, currentRadius);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        => Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
}
