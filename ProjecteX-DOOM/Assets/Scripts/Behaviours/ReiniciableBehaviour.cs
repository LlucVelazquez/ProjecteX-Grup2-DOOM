using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReiniciableBehaviour : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _mesh;
    private List<MeshRenderer> _meshes;

    public void SetValues(Collider collider = null, MeshRenderer mesh = null, MeshRenderer[] meshes = null)
    {
        _collider = collider;
        _mesh = mesh;
        _meshes = meshes.ToList();
    }

    public void DisableObject()
    {
        if (_collider != null) _collider.enabled = false;
        if (_mesh != null) _mesh.enabled = false;
        _meshes?.ForEach(m => m.enabled = false);
    }

    public void ResetObject()
    {
        if (_collider != null) _collider.enabled = true;
        if (_mesh != null) _mesh.enabled = true;
        _meshes?.ForEach(m => m.enabled = true);
    }
}
