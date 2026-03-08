using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    [Header("Footsetps")]
    [SerializeField] private float _walkStepInterval = 1f;
    [SerializeField] private float sprintStepInterval = 2f;

    private float _footstepTimer;

    public void PlayPlayerFootsteps(bool isGrounded, bool isMoving, bool isSprinting)
    {
        if (!isGrounded || isMoving)
        {
            _footstepTimer = 0f;
            return;
        }

        float stepInterval = isSprinting ? sprintStepInterval : _walkStepInterval;

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0f)
        {
            if (!isSprinting)
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerFootsteps, 0.2f);
            }
            else
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerSprintFootsteps, 0.2f);
            }
            _footstepTimer = stepInterval;
        }
    }
}
