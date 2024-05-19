using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Animator imageAnimator;

    public void Quit()
    {
        image.raycastTarget = true;
        imageAnimator.Play("ImageFadeIn");
        StartCoroutine(WaitAndLoad(1f));

    }

    IEnumerator WaitAndLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}