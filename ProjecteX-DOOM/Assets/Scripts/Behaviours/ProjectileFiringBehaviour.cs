using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFiringBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float shootInterval = 3f;
    [SerializeField] private float projectileSpeed = 2f;

    public static Stack<GameObject> ProjectileStack = new Stack<GameObject>();

    private void Shoot(Transform shotPoint)
    {

    }

    public void StackPush(GameObject go)
    {
        ProjectileStack.Push(go);
        go.SetActive(false);
    }

    private void StackPop(Transform shotPoint)
    {
        GameObject go = ProjectileStack.Pop();

        go.SetActive(true);

        go.transform.position = shotPoint.position;
        go.transform.rotation = shotPoint.rotation;
    }
}
