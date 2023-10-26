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
    [SerializeField] private TextMeshProUGUI ammoLeft;
    [SerializeField] private float maxHealth;
    private int currentAmmo;
    private float maxAmmo;
    private int ammoToAdd;
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
        currentAmmo = 20;
        maxAmmo = 20;
        ammoLeft.text = "Ammo: " + currentAmmo;
    }



    // Update is called once per frame

    void Update()

    {

        transform.position += transform.rotation * (speed * Time.deltaTime * _moveDir);
        
        CheckGround();

        // LATER ON: Use this logic to deal damage to player: Health -= Enemy.dealtDamage (or something..)
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
        // If there is ammo left, create a new projectile and shoot it
        if (currentAmmo > 0)
        { 
            // Create a new projectile
            Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Apply a force to the projectile to make it move forward
            currentProjectile.AddForce(lookAtPoint.forward * bulletForce, ForceMode.Impulse);
            
            // Destroy the projectile after 4 seconds
            Destroy(currentProjectile.gameObject, 4);
            // Decrease the ammo counter
            --currentAmmo;
            // Update the ammoLeft text
            ammoLeft.text = "Ammo: " + currentAmmo.ToString();
        }
        else
        {
            ammoLeft.text = "Press R to reload!";
        }

        // Log the maxAmmo and currentAmmo
        Debug.Log("maxAmmo: " + maxAmmo + " currentAmmo: " + currentAmmo);

    }

   public void Reload()
    {
        //If the ammo counter is less than the max ammo
        if (currentAmmo < maxAmmo)
        {
            //Set the ammo to add to the difference between the max ammo and the current ammo counter
            ammoToAdd = (int)maxAmmo - currentAmmo;
         
            //Add the ammo to the current ammo counter
            currentAmmo += ammoToAdd;

            //Set the text of the ammo left to the new ammo counter
            ammoLeft.text = "Ammo: " + currentAmmo.ToString();
        }
    }

    public void ammoCollected()
    {
    int maxAmmoDifference = (int)maxAmmo - currentAmmo;
    
    if (maxAmmoDifference > 0)
    {
        // Set the amount of ammo to add, limited by the difference between maxAmmo and currentAmmo
        ammoToAdd = Mathf.Min(5, maxAmmoDifference);
        
        // Add the ammo to the current ammo counter
        currentAmmo += ammoToAdd;
        
        // Set the text of the ammo left to the new ammo counter
        ammoLeft.text = "Ammo: " + currentAmmo.ToString();
    }
}


}