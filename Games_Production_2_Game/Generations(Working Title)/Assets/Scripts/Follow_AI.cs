using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_AI : MonoBehaviour
{
    [Header("References")]
    public GameObject target;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        transform.position = transform.forward * Time.deltaTime * moveSpeed;
    }
}
