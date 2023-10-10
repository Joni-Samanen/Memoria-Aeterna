using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private static int count = 0;

    private void OnTriggerEnter(Collider other)
    
    {
        if(other.transform.tag == "Player")
        {   
            count++;
            Destroy(gameObject);
            Debug.Log("Coins collected: " + count);
        }
    }
}
