using System.Collections;
using TMPro;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;



public class Player : MonoBehaviour

{


    private Vector2 currentRotation;
    [SerializeField] private Transform lookAtPoint;
    [SerializeField, Range(1,20)] private float mouseSensX;
    [SerializeField, Range(1,20)] private float mouseSensY;
    [SerializeField, Range(-90,0)] private float minViewAngle;
    [SerializeField, Range(0,90)] private float maxViewAngle;

    private Vector2 currentAngle;
    [SerializeField] private Transform followTarget;
    // Fields to store the player's speed and jump force
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    // Fields to store the layer mask for the ground
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float bulletForce;

    [Header("Player UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI shotsFired;
    [SerializeField] private float maxHealth;
    private int shotsFiredCounter;
    private float _health;
    private float Health 
    {
        get => _health;
        set 
        {
            _health = value;
            healthBar.fillAmount = _health / maxHealth;
        }
    }


    // Field to store the direction the player is moving

    // Field to store if the player is on the ground
    private bool isGrounded;
    // Field to store the direction the player is moving
    private Vector3 _moveDir;
    // Field to store the rigidbody of the player
    private Rigidbody rb;
    // Field to store the depth of the player
    private float depth;

    // Keep track of collected coins
    private int coins = 0;

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
        Health = maxHealth;
    }



    // Update is called once per frame

    void Update()

    {

        // Move the player
        transform.position += transform.rotation * (speed * Time.deltaTime * _moveDir);
        // Check if the player is on the ground
        CheckGround();

        Health -= Time.deltaTime * 5;

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
        // Controls rotation angles
        currentAngle.x += readValue.x * Time.deltaTime * mouseSensX;
        currentAngle.y += readValue.y * Time.deltaTime * mouseSensY;

        // Rotates left and right
        transform.rotation = Quaternion.AngleAxis(currentAngle.x, Vector3.up);
        followTarget.localRotation = Quaternion.AngleAxis(currentAngle.y, Vector3.right);

        //Clamp rotation angle so you can't roll your head
        currentAngle.y = Mathf.Clamp(currentAngle.y, minViewAngle, maxViewAngle);


    }
    public void Shoot()
    {
        Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        currentProjectile.AddForce(lookAtPoint.forward * bulletForce, ForceMode.Impulse);
        
        Destroy(currentProjectile.gameObject, 4);
        ++shotsFiredCounter;
        shotsFired.text = shotsFiredCounter.ToString();

    }
    public void coinsCollected()
    {
        coins++;
        Debug.Log("Coins collected: " + coins);
    }

    // Fields to store the player's speed and jump force



}