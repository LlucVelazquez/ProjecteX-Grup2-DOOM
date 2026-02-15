using UnityEngine;

[RequireComponent(typeof(FollowBehaviour))]
public class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private float _health = 10f;

    public float Health { get => _health; set => _health = value; }

    private FollowBehaviour _fb;

    private void Awake()
    {
        _fb = GetComponent<FollowBehaviour>();
    }

    private void Update()
    {
        _fb.FollowTarget(GameManager.Instance.Player.transform);
    }
}
