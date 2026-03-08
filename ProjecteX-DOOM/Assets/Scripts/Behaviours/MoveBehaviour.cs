using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveBehaviour : MonoBehaviour
{
    public Vector3 Velocity { get; set; }

    [SerializeField] private float _moveSpeed = 10f;

    [HideInInspector] public Vector3 moveDirection;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void SetMoveDirection(Camera cam, float x, float y)
    {
        Vector3 cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;

        moveDirection = cameraForward * y + cameraRight * x;
    }

    public void SetMoveDirection(CinemachineCamera cmCam, float x, float y)
    {
        Vector3 cameraForward = new Vector3(cmCam.transform.forward.x, 0, cmCam.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cmCam.transform.right.x, 0, cmCam.transform.right.z).normalized;

        moveDirection = cameraForward * y + cameraRight * x;
    }

    public void SetVerticalVelocity(float verticalVelocity)
    {
        var velocity = Velocity;
        velocity.y = verticalVelocity;
        Velocity = velocity;
    }

    public void MoveCharacter()
    {
        Vector3 velocity = moveDirection.normalized * _moveSpeed;
        velocity.y = Velocity.y;

        Velocity = velocity;

        _controller.Move(Velocity * Time.deltaTime);
    }

    public void MoveCharacter(float speed)
    {
        Vector3 velocity = moveDirection.normalized * speed;
        velocity.y = Velocity.y;

        Velocity = velocity;

        _controller.Move(Velocity * Time.deltaTime);
    }
}