using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UIElements;

public class Follow_AI : MonoBehaviour
{
    [Header("References")]
    public GameObject target;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    private float DistanceBetweenObjects;
    [SerializeField] private float AllyDistance;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        DistanceBetweenObjects = Vector2.Distance(transform.position, target.transform.position);
        Stop();
    }

    public void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        if(transform.position.magnitude > 0.1)
        {
            float angle = Mathf.Sin(Time.time * rotationSpeed) * 8;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        if(Dialogue_Manager.GetInstance().dialogueIsPlaying)
        {
            moveSpeed = 0;
            rotationSpeed = 0;
            transform.rotation = Quaternion.AngleAxis(0f, new Vector2(0, 0));
            return;
        }

        else
        {
            moveSpeed = 2f;
            rotationSpeed = 10f;
        }
       
    }

    public void Stop()
    {
        if(DistanceBetweenObjects <= AllyDistance)
        {
            moveSpeed = 0;
            rotationSpeed = 0;
            transform.rotation = Quaternion.AngleAxis(0f, new Vector2(0, 0));
        }
    }

}
