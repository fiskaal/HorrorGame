using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class QuestSystem : MonoBehaviour
{
    public GameObject hideCurrentQuest;
    public GameObject showNextQuest;
    public Animator animator;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] [TextArea] private string questText;
    [SerializeField] private UnityEvent newQuestSoundEvent;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("QuestAnim", 0, 0.0f);
            newQuestSoundEvent.Invoke();
            noteTextAreaUI.text = questText;
            hideCurrentQuest.SetActive(false);
            showNextQuest.SetActive(true);
            

        }

    }
}