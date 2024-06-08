using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator imageAnimator;
    public bool isGamePaused = false;
    [SerializeField] private GameObject crossHair;
    public bool noteOpened = false;
    public bool inventoryOpened = false;

    public void Start()
    {
        StartCoroutine(WaitAndPlay(1f));
    }


    // Update is called once per frame
    void Update()
    {

        //if (Input.GetButtonDown("Cancel"))
        //{
        //    Time.timeScale = 0;
        //    pauseMenu.SetActive(true);
        //}

        if (Input.GetKeyDown(KeyCode.Escape) && !noteOpened && !inventoryOpened)
        {
            if (isGamePaused)
            {
                ContinueGame();
                Debug.Log("Game continues");
            }
            else
            {
                //PauseGame();
                imageAnimator.Play("Image50FI");
                StartCoroutine(WaitForUI(.5f));
                Debug.Log("Game paused");
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        crossHair.gameObject.SetActive(false);
        Time.timeScale = 0;
        isGamePaused = true;
        //imageAnimator.Play("Image50FadeIn");

    }
    public void ContinueGame()
    {
        pauseMenu.gameObject.SetActive(false);
        crossHair.gameObject.SetActive(true);
        Time.timeScale = 1;
        isGamePaused = false;
        imageAnimator.Play("Image50FO");
    }

    IEnumerator WaitAndPlay(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1;
        PauseGame();
        ContinueGame();

    }

    IEnumerator WaitForUI(float waitTime)
    {
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 0;
        PauseGame();
        

    }
}
