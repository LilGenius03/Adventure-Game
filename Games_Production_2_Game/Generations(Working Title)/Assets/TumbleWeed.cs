using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleWeed : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f; 

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        float rotationAngle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
    }
}
