using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionSound;  // The sound to be played on collision
    public AudioSource audioSource;   // Reference to the AudioSource component
    public GameObject itself;



    void Start()
    {
        // Initialize the AudioSource component
        

        // Set the collision sound to the AudioSource
        audioSource.clip = collisionSound;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involves the desired object
        if (collision.gameObject.CompareTag("Player"))  // Replace "YourTag" with the tag of the object you want to collide with
        {
            // Play the collision sound
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
            audioSource.Play();
            itself.SetActive(false);
        }

    }
}