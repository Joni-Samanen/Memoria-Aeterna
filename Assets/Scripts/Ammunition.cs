using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {   
            // Call the ammoCollected method on the Player component
            other.GetComponent<Player>().AmmoCollected();
            
            // Destroy the ammo pickup game object
            Destroy(gameObject);
        }
    }
}