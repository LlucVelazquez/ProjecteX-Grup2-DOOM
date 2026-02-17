using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    public IInteractable Interactable { get => _interactable; set => _interactable = value; }

    [SerializeField] private Camera _camera;
    [SerializeField] private float _interactDistance = 2f;

    private IInteractable _interactable;

    public void CheckInteraction()
    {
        Vector3 origin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(origin, _camera.transform.forward, Color.red);

        if (Physics.Raycast(origin, _camera.transform.forward, out RaycastHit hit, _interactDistance))
        {
            _interactable = hit.collider.GetComponent<IInteractable>();
        }
    }
}
