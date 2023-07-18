using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 direction = target.forward;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}