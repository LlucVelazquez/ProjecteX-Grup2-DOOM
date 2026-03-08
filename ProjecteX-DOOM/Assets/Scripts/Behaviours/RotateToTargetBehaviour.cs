using UnityEngine;

public class RotateToTargetBehaviour : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = DistanceUtils.GetDirection(transform.position, targetPosition);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                             rotationSpeed * Time.deltaTime);
    }
}
