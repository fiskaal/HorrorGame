using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
//using Unity.UI;
using System;

public class LPOnCollision : MonoBehaviour
{
    // The name of the scene you want to load
    public string sceneName = "TUTORIAL_level";
    public GameObject LoadingPage;
    public Slider slider;

    // The spawn coordinates for the player
    public Vector3 spawnCoordinates;

    // This function is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Set the player's position to the spawn coordinates
            other.transform.position = spawnCoordinates;

            // Load the specified scene
            //SceneManager.LoadScene(sceneName);

            StartCoroutine(LoadAsynchro(sceneName));
        }
    }

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
