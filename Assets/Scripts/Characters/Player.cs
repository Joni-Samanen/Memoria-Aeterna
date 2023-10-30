using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Movement
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayers;
    private bool isGrounded;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private float depth;

    // Look rotation
    [SerializeField] private Transform lookAtPoint;
    [SerializeField, Range(1, 20)] private float mouseSensX;
    [SerializeField, Range(1, 20)] private float mouseSensY;
    [SerializeField, Range(-90, 0)] private float minViewAngle;
    [SerializeField, Range(0, 90)] private float maxViewAngle;
    private Vector2 currentAngle;
    [SerializeField] private Transform followTarget;

    // Shooting
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float bulletForce;
    [SerializeField] private int maxAmmo;
    private int currentAmmo;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI ammoLeft;
    [SerializeField] private float maxHealth;
    private float health;
    private int ammoToAdd;

    private void Start()
    {
        // Initialize input manager
        InputManager.Init(this);
        InputManager.GameMode();

        // Get components
        rb = GetComponent<Rigidbody>();
        depth = GetComponent<Collider>().bounds.size.y;

        // Set initial values
        health = maxHealth;
        currentAmmo = maxAmmo;
        ammoLeft.text = "Ammo: " + currentAmmo;
    }

    private void Update()
    {
        // Move player
        transform.position += transform.rotation * (speed * Time.deltaTime * moveDirection);

        // Check if player is on the ground
        CheckGround();

        // LATER ON: Use this logic to deal damage to player: Health -= Enemy.dealtDamage (or something..)
    }

    public void SetMovementDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, depth, groundLayers);
        Debug.DrawRay(transform.position, Vector3.down * depth, Color.green, 0, false);
    }

    public void SetLookRotation(Vector2 readValue)
    {
        currentAngle.x += readValue.x * Time.deltaTime * mouseSensX;
        currentAngle.y += readValue.y * Time.deltaTime * mouseSensY;

        transform.rotation = Quaternion.AngleAxis(currentAngle.x, Vector3.up);
        followTarget.localRotation = Quaternion.AngleAxis(currentAngle.y, Vector3.right);

        currentAngle.y = Mathf.Clamp(currentAngle.y, minViewAngle, maxViewAngle);
    }
    public void Shoot()
    {
        
        if (currentAmmo > 0)
        {
            Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            currentProjectile.AddForce(lookAtPoint.forward * bulletForce, ForceMode.Impulse);
            Collider playerCollider = GetComponent<Collider>();
            Collider projectileCollider = currentProjectile.GetComponent<Collider>();
            Physics.IgnoreCollision(playerCollider, projectileCollider);
            Destroy(currentProjectile.gameObject, 4);

            --currentAmmo;
            ammoLeft.text = "Ammo: " + currentAmmo.ToString();
        }
        else
        {
            ammoLeft.text = "Press R to reload!";
        }
    }

    public void Reload()
    {
        int ammoToAdd = (int)maxAmmo - currentAmmo;

        if (ammoToAdd > 0)
        {
            currentAmmo += ammoToAdd;
            ammoLeft.text = "Ammo: " + currentAmmo.ToString();
        }
    }

    public void AmmoCollected()
    {
        int maxAmmoDifference = (int)maxAmmo - currentAmmo;

        if (maxAmmoDifference > 0)
        {
            int ammoToAdd = Mathf.Min(5, maxAmmoDifference);
            currentAmmo += ammoToAdd;
            ammoLeft.text = "Ammo: " + currentAmmo.ToString();
        }
    }


}