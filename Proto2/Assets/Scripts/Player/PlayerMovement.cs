/*****************************************************************************
// File Name :         PlayerMovement.cs
// Author :            Lorien Nergard
// Creation Date :     February 16th, 2024
//
// Brief Description : Controls the movement of the player
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerInput;
    public PlayerLookBehavior playerLookBehavior;

    public Transform orientation;
    float horizontalInput;
    float verticaleInput;

    private Vector3 moveDirection;
    private Vector3 respawnPos;

    [SerializeField] GameObject Cam;

    private Rigidbody rb;
    public float moveSpeed;
    public float launchSpeed;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float groundDrag;

    public float currentReloadShots;
    public float maxReloadShots;
    public float reloadTime;
    private float reloadCounter;
    public float xRespawnRotation = -8, yRespawnRotation = -360;

    [SerializeField] public Image ReloadUI;
    [SerializeField] TMP_Text reloadText, checkpointText;

    public GameObject gameController;



    /// <summary>
    /// Start function
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentReloadShots = maxReloadShots;
        reloadText.SetText(maxReloadShots.ToString());

        respawnPos = transform.position;
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
            Debug.Log("Goober");
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if(currentReloadShots < maxReloadShots)
        {
            reloadCounter += Time.deltaTime;

            if(reloadCounter >= reloadTime)
            {
                currentReloadShots++;
                reloadCounter = 0;
                reloadText.SetText(currentReloadShots.ToString());
            }
        }

        if(grounded && currentReloadShots == 0)
        {
            currentReloadShots = 1;
            reloadText.SetText(currentReloadShots.ToString());
        }

        ReloadUI.fillAmount = reloadCounter / reloadTime;

        /*if (gameController.GetComponent<GameController>().isPaused)
        {
            playerInput.Player.Shoot.performed -= Shoot;
        }
        else
        {
            playerInput.Player.Shoot.performed += Shoot;
        }*/
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
        SpeedControl();
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

        if(flatVel.magnitude > moveSpeed)   
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }
    

    /// <summary>
    /// The shoot action propels the player
    /// </summary>
    /// <param name="context"></param>
    private void Shoot(InputAction.CallbackContext context)
    {  
        if(currentReloadShots > 0)
        {
            Vector3 lauchDirection = orientation.transform.rotation.eulerAngles;
            lauchDirection.x = (lauchDirection.x + 180) % 360;
            lauchDirection.y = (lauchDirection.y + 180) % 360;
            //rb.AddForce(0,1,0, ForceMode.Impulse);
            rb.AddForce(Cam.transform.forward * -1 * launchSpeed * 10f, ForceMode.Impulse);


            Vector3 launchFlatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (launchFlatVel.magnitude > launchSpeed)
            {
                Vector3 limitedLaunchVel = launchFlatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedLaunchVel.x, rb.velocity.y, limitedLaunchVel.z);
            }

            currentReloadShots--;
            reloadText.SetText(currentReloadShots.ToString());
        }

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Respawn Zone")
        {
            transform.position = respawnPos;
            playerLookBehavior.xRotation = xRespawnRotation;
            playerLookBehavior.yRotation = yRespawnRotation;
        }
        if (collision.gameObject.name == "Checkpoint 1")
        {
            respawnPos = new Vector3(3.53999996f, 13.9300003f, 104.769997f);
            checkpointText.enabled = true;
            StartCoroutine(DisplayCheckpointText());
            Destroy(GameObject.Find("Checkpoint 1"));
        }
        if (collision.gameObject.name == "Checkpoint 2")
        {
            respawnPos = new Vector3(173, 5.69999981f, 138.199997f);
            xRespawnRotation = -5;
            yRespawnRotation = 90;
            checkpointText.enabled = true;
            StartCoroutine(DisplayCheckpointText());
            Destroy(GameObject.Find("Checkpoint 2"));
        }
    }

    IEnumerator DisplayCheckpointText()
    {
        yield return new WaitForSeconds(2);
        checkpointText.enabled = false;
    }

    private void OnEnable()
    {
        playerInput = new PlayerControls(); 

        playerInput.Enable();
        playerInput.Player.Shoot.performed += Shoot;
        playerInput.Player.Pause.performed += Pause;
    }

    private void OnDisable()
    {
      playerInput.Disable();
      playerInput.Player.Shoot.performed -= Shoot;
      playerInput.Player.Pause.performed -= Pause;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        gameController.GetComponent<GameController>().Pause();
        Debug.Log("gamer");
    }

}
