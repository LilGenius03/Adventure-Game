using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Script : MonoBehaviour
{
    [Header("Settings")]
    public float Speed;

    [Header("References")]
    private Rigidbody2D rb;
    private Vector2 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * Speed * Time.deltaTime;
        transform.Rotate(0, 0,-Input.GetAxis("Horizontal"));

    }
}
