using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _camera;

    [Header("Interact Settings")]
    [SerializeField] private float _interactDistance = 2f;

    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        Vector3 origin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(origin, _camera.transform.forward, Color.red);

        if (Physics.Raycast(origin, _camera.transform.forward, out RaycastHit hit, _interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (_playerInputs.Interact)
                {
                    interactable.OnInteract(gameObject);
                }
            }
        }
    }
}
