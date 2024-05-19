using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image image;
    [SerializeField] private Animator imageAnimator;
    public float waitTime = 1f;
    public string sceneName;
   
    

    public void Awake()
    {
        image.raycastTarget = false;
    }

    public void LoadGameScene(string sceneName)
    {
        image.raycastTarget = true;
        imageAnimator.Play("ImageFadeIn");
        StartCoroutine(WaitAndLoad(waitTime, sceneName));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MM");
    }

    public void TestAnimation(string sceneName)
    {
        image.raycastTarget = true;
        imageAnimator.Play("ImageFadeIn");
        StartCoroutine(WaitAndLoad(waitTime, sceneName));

    }

    public void FadeInImage()
    {
        image.raycastTarget = true;
    }

    public void FadeOutImage()
    {

    }

    IEnumerator WaitAndLoad(float waitTime, string sceneName)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
        //SceneManager.LoadSceneAsync(sceneName);
        //AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    }
}
