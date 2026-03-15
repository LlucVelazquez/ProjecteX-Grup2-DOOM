using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SecretRoom : MonoBehaviour
{
    [SerializeField] private Vector3 _openPosition;
    [SerializeField] private NavMeshAgent _agentOnPlatform;

    private Vector3 _initialPosition;
    private Vector3 _agentInitialPosition;
    private NavMeshSurface _surface;

    private void Awake()
    {
        _initialPosition = transform.position;

        if (_agentOnPlatform != null) _agentInitialPosition = _agentOnPlatform.transform.position;
        _surface = GetComponent<NavMeshSurface>();
    }

    public void Open()
    {
        transform.position += _openPosition;

        StartCoroutine(RebakeSurface());

        if (_agentOnPlatform != null)
        {
            _agentOnPlatform.enabled = false;
            _agentOnPlatform.transform.position += _openPosition;
            _agentOnPlatform.Warp(_agentOnPlatform.transform.position);
            _agentOnPlatform.enabled = true;
        }
    }
    public void Close()
    {
        transform.position = _initialPosition;

        StartCoroutine(RebakeSurface());

        if (_agentOnPlatform != null)
        {
            _agentOnPlatform.enabled = false;
            _agentOnPlatform.transform.position = _agentInitialPosition;
            _agentOnPlatform.Warp(_agentOnPlatform.transform.position);
            _agentOnPlatform.enabled = true;
        }
    }

    private IEnumerator RebakeSurface()
    {
        yield return _surface.UpdateNavMesh(_surface.navMeshData);
    }
}
