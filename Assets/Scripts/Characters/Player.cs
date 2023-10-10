using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Player : MonoBehaviour

{

    // Fields to store the player's speed and jump force
    [SerializeField, Range(1,20)] private float speed;
    [SerializeField, Range(1, 20)] private float jumpForce;
    // Fields to store the layer mask for the ground
    [SerializeField] private LayerMask groundLayers;
    // Field to store the direction the player is moving
    private float    

    // Field to store if the player is on the ground
    private bool isGrounded;
    // Field to store the direction the player is moving
    private Vector3 _moveDir;
    // Field to store the rigidbody of the player
    private Rigidbody rb;
    // Field to store the depth of the player
    private float depth;
    private Vector2 currentRotation;
    [SerializeField] private float mouseSensX;
    [SerializeField] private float mouseSensY;

    // Start is called before the first frame update

    void Start()

    {
        // Initialize the input manager
        InputManager.Init(this);
        // Set the game mode to player mode
        InputManager.GameMode();

        // Get the rigidbody of the player
        rb = GetComponent<Rigidbody>();
        // Get the depth of the player
        depth = GetComponent<Collider>().bounds.size.y;
    }



    // Update is called once per frame

    void Update()

    {

        // Move the player
        transform.position += speed * Time.deltaTime * _moveDir;
        // Check if the player is on the ground
        CheckGround();

    }



    // Method to set the direction the player is moving
    public void SetMovementDirection(Vector3 newDirection)

    {
        _moveDir = newDirection;
    }

    // Method to jump when the player is on the ground
    public void Jump()
    {
        Debug.Log("Jump called");
        // Check if the player is on the ground
        if (isGrounded)
        {
            Debug.Log("I jumped!");
            // Add a force to the player to jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Method to check if the player is on the ground
    private void CheckGround()
    {
        // Check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, depth, groundLayers);
        // Draw a ray from the player to the ground
        Debug.DrawRay(transform.position, Vector3.down * depth, Color.green, 0, false);
    }

    public void SetLookRotation(Vector2 readValue)
    {

    }
    public void Shoot()
    {
        
    }


    // Fields to store the player's speed and jump force



}