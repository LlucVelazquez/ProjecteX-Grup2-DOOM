using Unity.Cinemachine;
using UnityEngine;

public class CamRotationBehaviour : MonoBehaviour
{
    public CinemachineCamera Camera { get => _cam; private set => _cam = value; }

    [SerializeField] private CinemachineCamera _cam;

    public float lookSense = 0.1f;
    public float lookLimitV = 89f;

    private Vector3 _cameraRotation;

    public void RotateCamera(Vector2 rotation, out float targetYRotation)
    {
        _cameraRotation.x += rotation.x * lookSense;
        _cameraRotation.y -= rotation.y * lookSense;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -lookLimitV, lookLimitV);

        _cam.transform.localRotation = Quaternion.Euler(_cameraRotation.y, 0f, 0f);
        targetYRotation = _cameraRotation.x;
    }

    public void UpdateSensitivity()
    {
        lookSense = PlayerPrefs.GetFloat(OptionSettingsUtils.SensitivityKey, OptionSettingsUtils.DefaultSensitivity);
    }
}
