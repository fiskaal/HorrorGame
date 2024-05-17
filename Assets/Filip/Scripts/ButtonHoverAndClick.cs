using System.Collections;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverAndClick : MonoBehaviour
{ 
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void HoverSound()
    {
        audioSource.PlayOneShot(hoverSound);


    }

    public void ClickSound()
    {
        audioSource.PlayOneShot(clickSound);


    }
   

    
}
