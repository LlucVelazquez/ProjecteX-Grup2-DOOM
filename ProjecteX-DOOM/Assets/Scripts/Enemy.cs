using UnityEngine;

public abstract class Enemy : MonoBehaviour, ITargeteable
{
    [SerializeField] private float _health = 10f;

    public float Health { get => _health; set => _health = value; }

    private void 
}
