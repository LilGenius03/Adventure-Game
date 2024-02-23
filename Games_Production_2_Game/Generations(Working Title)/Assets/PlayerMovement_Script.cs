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
    [SerializeField] private GameObject Map;

    [Header("Bools")]
    private bool MapOpen;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Map.SetActive(false);
        MapOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * Speed * Time.deltaTime;
        //transform.Rotate(0, 0,-Input.GetAxis("Horizontal"));

        if(Input.GetKeyDown(KeyCode.M) && MapOpen == false)
        {
            Map.SetActive(true);
            MapOpen = true;
        }

        else if(Input.GetKeyDown(KeyCode.M) && MapOpen == true) 
        {
            MapOpen = false;
            Map.SetActive(false);
        }

    }
}
