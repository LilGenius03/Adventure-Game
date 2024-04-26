using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Script : MonoBehaviour
{
    [Header("Settings")]
    public float Speed;
    [SerializeField] float rotationSpeed;

    [Header("References")]
    private Rigidbody2D rb;
    private Vector2 moveDir;
    [SerializeField] private GameObject Map;
    public KeyCode interact;
    public KeyCode Submit;

    [Header("Bools")]
    private bool MapOpen;
    public bool keyPressed;
    public bool SubmitKeyPressed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Map.SetActive(false);
        MapOpen = false;
        keyPressed = false;
        SubmitKeyPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(interact) && keyPressed == false)
        {
            Debug.Log("F");
            keyPressed = true;
        }

        else
        {
            keyPressed = false;
        }

        if(Input.GetKeyDown(Submit) && SubmitKeyPressed == false)
        {
            SubmitKeyPressed = true;
        }

        else
        {
            SubmitKeyPressed = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * Speed * Time.deltaTime;

        if(rb.velocity.sqrMagnitude > 0.1)
        {
            float angle = Mathf.Sin(Time.time * rotationSpeed) * 8;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if(Dialogue_Manager.GetInstance().dialogueIsPlaying)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rotationSpeed = 0f;
            transform.rotation = Quaternion.AngleAxis(0f, new Vector2(0,0));
            return;
        }

        /*if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.AngleAxis(0f, new Vector2(0, 0));
        }*/

        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rotationSpeed = 10f;
        }

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
