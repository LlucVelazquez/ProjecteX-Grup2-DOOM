using UnityEngine;

public class SuicideEffectBehaviour : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!_ps.isPlaying) Destroy(gameObject);
    }
}
