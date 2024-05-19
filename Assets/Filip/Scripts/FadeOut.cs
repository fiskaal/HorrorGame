using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Animator imageAnimator;
    public float waitTime = 1f;
    // Start is called before the first frame update

    private void Awake()
    {

        Time.timeScale = 0;
        image.raycastTarget = true;
        
    }
    void Start()
    {
       
        imageAnimator.Play("ImageFadeOut");
        StartCoroutine(WaitAndPlay(waitTime));
       
    }



    IEnumerator WaitAndPlay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        image.raycastTarget = false;

    }
}
