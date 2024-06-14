using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class QuestSystem : MonoBehaviour
{
    public AudioClip newQuestSound;  // The sound to be played on collision
    public AudioSource audioSource;   // Reference to the AudioSource component
    public GameObject hideCurrentQuest;
    public GameObject showNextQuest;
    //public GameObject currentQuestText;
    //public GameObject nextQuestText;

    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] [TextArea] private string questText;
    [SerializeField] private UnityEvent newQuestSoundEvent;

    private void Awake()
    {
        //noteTextAreaUI.text = null;
    }

    void Start()
    {
        // Initialize the AudioSource component


        // Set the collision sound to the AudioSource
        //audioSource.clip = newQuestSound;
        //noteTextAreaUI.text = "";
    }

    





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            newQuestSoundEvent.Invoke();
            noteTextAreaUI.text = questText;
            //audioSource.Play();
            hideCurrentQuest.SetActive(false);
            //currentQuestText.SetActive(false);
            showNextQuest.SetActive(true);
            //nextQuestText.SetActive(true);
            
            


        }

    }
}