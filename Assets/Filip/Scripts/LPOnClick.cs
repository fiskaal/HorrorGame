using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
//using Unity.UI;
using System;

public class LPOnClick : MonoBehaviour
{
    // The name of the scene you want to load
    public string sceneName = "TUTORIAL_level";
    public GameObject LoadingPage;
    public Slider slider;

    // The spawn coordinates for the player
    public Vector3 spawnCoordinates;

    // This function is called when the Collider other enters the trigger
    

    public void LoadLevel(string nameOfTheScene)
    {
        StartCoroutine(LoadAsynchro(nameOfTheScene));

    }

    IEnumerator LoadAsynchro(string nameOfTheScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nameOfTheScene);

        LoadingPage.SetActive(true);

        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            slider.value = progress;
            yield return null;
        }

    }
}
