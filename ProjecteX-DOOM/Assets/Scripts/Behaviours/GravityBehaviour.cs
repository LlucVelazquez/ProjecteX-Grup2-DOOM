using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravityBehaviour : MonoBehaviour
{
    public float TimeSinceLastGrounded { get; private set; } = 0f;

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _physicsSphereRadiusOffset = 0.1f;
    [SerializeField] private float _verticalVelocityLimit = 20f;

    public float gravity = 9.81f;
    [HideInInspector] public bool isGrounded;

    public bool drawGizmos = true;

    private CharacterController _controller;
    private MoveBehaviour _mb;

    private Vector3 _spherePosition;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        _mb = GetComponent<MoveBehaviour>();
    }

    private void Update()
    {
        if (isGrounded)
        {
            TimeSinceLastGrounded = 0f;
        }
        else
        {
            TimeSinceLastGrounded += Time.deltaTime;
        }
    }

    public void UpdateGravity()
    {
        GroundCheck();
        ApplyGravity();
    }

    public void GroundCheck()
    {
        _spherePosition = new Vector3(transform.position.x, transform.position.y / 2 - _controller.radius / 2, transform.position.z);
        isGrounded = Physics.CheckSphere(_spherePosition, _controller.radius - _physicsSphereRadiusOffset, _groundLayers);
    }

    public void ApplyGravity()
    {
        float verticalVelocity = _mb.Velocity.y;

        verticalVelocity -= gravity * Time.deltaTime;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (verticalVelocity < -_verticalVelocityLimit)
        {
            verticalVelocity = -_verticalVelocityLimit;
        }

        _mb.SetVerticalVelocity(verticalVelocity);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            try
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_spherePosition, _controller.radius - _physicsSphereRadiusOffset);
            }
            catch
            {
                // Ignore exceptions when the component is not properly initialized
            }
        }
    }
}