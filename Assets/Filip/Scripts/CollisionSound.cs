using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CollisionSound : MonoBehaviour
{
    public GameObject hideItself;
    public GameObject showNextTrigger;
    [SerializeField] private UnityEvent newQuestSoundEvent;



    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
            
            hideItself.SetActive(false);
            newQuestSoundEvent.Invoke();
            showNextTrigger.SetActive(true);
        }

    }
}