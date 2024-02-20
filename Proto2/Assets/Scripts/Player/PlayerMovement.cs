/*****************************************************************************
// File Name :         PlayerMovement.cs
// Author :            Lorien Nergard
// Creation Date :     February 16th, 2024
//
// Brief Description : Controls the movement of the player
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;

    public Transform orientation;
    float horizontalInput;
    float verticaleInput;
    Vector3 moveDirection;
    Rigidbody rb;
    public float moveSpeed;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float groundDrag;
   

    /// <summary>
    /// Start function
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerInput = new PlayerInput();
    }

    /// <summary>
    /// Detects the ground to apply drag and runs the AxisInput function 
    /// </summary>
    void Update()
    {
        AxisInput();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    /// <summary>
    /// Calls the run function for the player to move
    /// </summary>
    private void FixedUpdate()
    {
        Run();
    }

    /// <summary>
    /// Allows the player to move in the direction that they are facing
    /// </summary>
    void Run()
    {
        moveDirection = orientation.forward * verticaleInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    /// <summary>
    /// Gets the axis' for movement 
    /// </summary>
    private void AxisInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticaleInput = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Prevents the player from exceeding the set speed
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)   //When making the gun adjust this to account for faster movement in the air
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// The shoot action propels the player into the air 
    /// </summary>
    /// <param name="context"></param>
    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");
        // Use add force except make moveDirection aim behind the player rather than forward 
        //Make a gameobject for it and have the game object be turned around so it goes in that directiom
        //Use a seperate variable for the shoot speed to better control it and not let it get too crazy
        /*
        moveDirection = orientation.forward * verticaleInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        */
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Shoot.performed += Shoot; 
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        playerInput.Player.Shoot.performed -= Shoot;
    }
}
