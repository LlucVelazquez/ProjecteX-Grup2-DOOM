using System.Collections.Generic;
using UnityEngine;

public class ProjectileFiringBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootInterval = 3f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int projectileDamageMulti = 3;
    [SerializeField] private int projectileMaxBaseDamage = 8;

    public static Stack<GameObject> ProjectileStack = new Stack<GameObject>();

    public string targetLayerName;

    private float _lastShootTime;

    public void Shoot(Vector3 direction)
    {
        if (Time.time - _lastShootTime >= _shootInterval)
        {
            if (ProjectileStack.Count == 0)
            {
                SpawnProjectile(direction);
            }
            else
            {
                ProjectileStackPop(direction);
            }

            _lastShootTime = Time.time;
        }
    }

    private void SpawnProjectile(Vector3 direction)
    {
        _projectile.GetComponent<ProjectileBehaviour>().shooter = this;
        _projectile.GetComponent<ProjectileBehaviour>().speed = projectileSpeed;
        _projectile.GetComponent<ProjectileBehaviour>().damageMulti = projectileDamageMulti;
        _projectile.GetComponent<ProjectileBehaviour>().maxBaseDamage = projectileMaxBaseDamage;
        _projectile.GetComponent<ProjectileBehaviour>().direction = direction;

        Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation);
    }

    public void ProjectileStackPush(GameObject go)
    {
        ProjectileStack.Push(go);
        go.SetActive(false);
    }

    private void ProjectileStackPop(Vector3 direction)
    {
        GameObject go = ProjectileStack.Pop();

        go.SetActive(true);

        go.transform.position = _shootPoint.position;
        go.transform.rotation = _shootPoint.rotation;

        go.GetComponent<ProjectileBehaviour>().direction = direction;
    }
}
