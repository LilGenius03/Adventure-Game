using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;
    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, damping);
    }
}
