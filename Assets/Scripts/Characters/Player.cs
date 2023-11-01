using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Movement
    [SerializeField] private float speed; // The speed at which the player moves
    [SerializeField] private float jumpForce; // The force applied when the player jumps
    [SerializeField] private LayerMask groundLayers; // The layers considered as ground
    private bool isGrounded; // Indicates if the player is grounded
    private Vector3 moveDirection; // The direction of movement
    private Rigidbody rb; // The player's Rigidbody component
    private float depth; // The distance to check for ground

    // Look rotation
    [SerializeField] private Transform lookAtPoint; // The point the player looks at
    [SerializeField, Range(1, 20)] private float mouseSensX; // The mouse sensitivity on the X axis
    [SerializeField, Range(1, 20)] private float mouseSensY; // The mouse sensitivity on the Y axis
    [SerializeField, Range(-90, 0)] private float minViewAngle; // The minimum view angle
    [SerializeField, Range(0, 90)] private float maxViewAngle; // The maximum view angle
    private Vector2 currentAngle; // The current rotation angle of the player's view
    [SerializeField] private Transform followTarget; // The target to follow

    // Shooting
    [SerializeField] private Rigidbody bulletPrefab; // The prefab of the bullet
    private int handgunAmmo; // The current ammo count
    private int shotgunAmmo; // The current ammo count
    private int maxShotgunAmmo; // The maximum ammo count
    private int maxHandgunAmmo; // The maximum ammo count
    [SerializeField] private Image healthBar; // The health bar UI element
    [SerializeField] private TextMeshProUGUI ammoLeft; // The UI element displaying the ammo count
    [SerializeField] private float maxHealth; // The maximum health value
    private float health; // The current health value
    private int ammoToAdd; // The amount of ammo to add


    public enum WeaponType
    {
        Handgun,
        Shotgun
    }

    private WeaponType currentWeapon; // The current weapon type

    private void Start()
    {
        InitializeInputManager();
        GetComponents();
        SetInitialValues();
    }

    private void Update()
    {
        MovePlayer();
        CheckGround();
    }

    
    /// Moves the player based on the movement direction.
    
    private void MovePlayer()
    {
        transform.position += transform.rotation * (speed * Time.deltaTime * moveDirection);
    }

    
    /// Checks if the player is grounded.
    
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, depth, groundLayers);
        Debug.DrawRay(transform.position, Vector3.down * depth, Color.green, 0, false);
    }

    
    /// Sets the movement direction of the player.
    
    /// <param name="newDirection">The new movement direction.</param>
    public void SetMovementDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }

    
    /// Makes the player jump if grounded.
    
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    public void SetLookRotation(Vector2 readValue)
    {
        // Update the current angle based on the input values and sensitivity
        currentAngle.x += readValue.x * Time.deltaTime * mouseSensX;
        currentAngle.y += readValue.y * Time.deltaTime * mouseSensY;

        // Rotate the main transform around the Y-axis
        transform.rotation = Quaternion.AngleAxis(currentAngle.x, Vector3.up);
        // Rotate the follow target transform around the X-axis
        followTarget.localRotation = Quaternion.AngleAxis(currentAngle.y, Vector3.right);

        // Clamp the Y-angle between the minimum and maximum view angles
        currentAngle.y = Mathf.Clamp(currentAngle.y, minViewAngle, maxViewAngle);
    }

    public void Shoot()
    {
        switch (currentWeapon)
        {
            case WeaponType.Handgun:
                if (handgunAmmo > 0)
                {
                    
                    ShootHandgun();
                    ammoLeft.text = string.Format("Handgun ammo: {0}", handgunAmmo);
                }
                break;

            case WeaponType.Shotgun:
                if (shotgunAmmo > 0)
                {
                    
                    ShootShotgun();
                    ammoLeft.text = string.Format("Shotgun ammo: {0}", shotgunAmmo);
                }
                break;
        }

    }

    private void ShootHandgun()
    {
        // Instantiate a bullet at the current position with no rotation
        Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // Apply force to the bullet in the direction of the lookAtPoint
        currentProjectile.AddForce(lookAtPoint.forward * GetBulletForce(), ForceMode.Impulse);
        // Get the collider component of the projectile
        Collider projectileCollider = currentProjectile.GetComponent<Collider>();
        handgunAmmo--;
        

        if (projectileCollider != null)
        {
            // Ignore collision between the player's collider and the projectile's collider
            Physics.IgnoreCollision(GetComponent<Collider>(), projectileCollider);
            // Destroy the projectile after 4 seconds
            Destroy(currentProjectile.gameObject, 4);
        }
    }

    private void ShootShotgun()
    {
        // Shoot 5 bullets in random directions
        for (int i = 0; i < 10; i++)
        {
            // Generate a random rotation for each bullet
            Vector3 randomRotation = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f));

            // Instantiate a bullet at the current position with the random rotation
            Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(randomRotation));
            // Apply force to the bullet in the direction of the lookAtPoint
            currentProjectile.AddForce(lookAtPoint.forward * GetBulletForce(), ForceMode.Impulse);
            // Get the collider component of the projectile
            Collider projectileCollider = currentProjectile.GetComponent<Collider>();
            shotgunAmmo--;

            if (projectileCollider != null)
            {
                // Ignore collision between the player's collider and the projectile's collider
                Physics.IgnoreCollision(GetComponent<Collider>(), projectileCollider);
                // Destroy the projectile after 4 seconds
                Destroy(currentProjectile.gameObject, 4);
            }
        }
    }

    
    /// Returns the force of the bullet based on the current weapon.
    
    /// <returns>The force of the bullet.</returns>
    private float GetBulletForce()
    {
        switch (currentWeapon)
        {
            case WeaponType.Handgun:
                return 100f;

            case WeaponType.Shotgun:
                return 50f;

            default:
                return 5f;
        }
    }

    
    /// Reloads the weapon by adding ammo if there is any left in the inventory.
    
    public void Reload()
    {
        switch (currentWeapon)
        {
            case WeaponType.Handgun:
                int handgunAmmoDifference = maxHandgunAmmo - handgunAmmo;
                if (handgunAmmoDifference > 0)
                {
                    handgunAmmo += handgunAmmoDifference;
                    ammoLeft.text = string.Format("Handgun Ammo: {0}", handgunAmmo);
                }
                break;

            case WeaponType.Shotgun:
                int shotgunAmmoDifference = maxShotgunAmmo - shotgunAmmo;
                if (shotgunAmmoDifference > 0)
                {
                    shotgunAmmo += shotgunAmmoDifference;
                    ammoLeft.text = string.Format("Shotgun Ammo: {0}", shotgunAmmo);
                }
                break;
        }
    }

    
    /// Switches the current weapon between handgun and shotgun.
    
    public void SwitchWeapon()
    {
        if (currentWeapon == WeaponType.Handgun)
        {
            currentWeapon = WeaponType.Shotgun;
            ammoLeft.text = string.Format("Shotgun Ammo: {0}", shotgunAmmo);
        }
        else
        {
            currentWeapon = WeaponType.Handgun;
            ammoLeft.text = string.Format("Handgun Ammo: {0}", handgunAmmo);
        }
    }

    
    /// Adds collected ammo to the inventory, up to a maximum limit.
    
    public void AmmoCollected()
    {
        int ammoDifference = maxHandgunAmmo - handgunAmmo;

        if (ammoDifference > 0)
        {
            int ammoToAdd = Mathf.Min(5, ammoDifference);
            handgunAmmo += ammoToAdd;
            ammoLeft.text = string.Format("Ammo: {0}", handgunAmmo);
        }
    }

    
    /// Initializes the input manager and sets the game mode.
    
    private void InitializeInputManager()
    {
        InputManager.Init(this);
        InputManager.GameMode();
    }

    
    /// Gets the necessary components for the player object.
    
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody>();
        depth = GetComponent<Collider>().bounds.size.y;
    }

    
    /// Sets the initial values for health, ammo, and UI text.
    
    private void SetInitialValues()
    {
        health = maxHealth;
        maxHandgunAmmo = 10;
        maxShotgunAmmo = 20;
        handgunAmmo = 10;
        shotgunAmmo = 20;
    }
}