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
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerInput = new PlayerInput();
    }

    // Update is called once per frame
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

    private void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        moveDirection = orientation.forward * verticaleInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void AxisInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticaleInput = Input.GetAxisRaw("Vertical");
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)   //When making the gun adjust this to account for faster movement in the air
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot");
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
