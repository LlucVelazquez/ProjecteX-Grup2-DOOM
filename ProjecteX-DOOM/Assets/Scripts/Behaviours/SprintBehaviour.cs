using UnityEngine;

[RequireComponent(typeof(MoveBehaviour))]
public class SprintBehaviour : MonoBehaviour
{
    [SerializeField] private float _sprintSpeed = 20f;

    private MoveBehaviour _mb;

    private void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
    }

    public void Sprint()
    {
        _mb.MoveCharacter(_sprintSpeed);
    }
}