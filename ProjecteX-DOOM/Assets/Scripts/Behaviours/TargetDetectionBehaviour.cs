using UnityEngine;

public class TargetDetectionBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _detectionRange = 10f;

    [Tooltip("By default adds gameObject and Target Layers")]
    [SerializeField] private LayerMask _ignoreLayers;

    public Vector3 TargetPosition => _target.transform.position;

    private void Awake()
    {
        _ignoreLayers |= (1 << gameObject.layer) | (1 << _target.layer);
    }

    public bool IsTargetWithinTheLineOfSight()
    => DistanceUtils.HasLineOfSight(transform.position, _target.transform.position, _detectionRange, _ignoreLayers);

}
