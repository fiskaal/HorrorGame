using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadFungusScene()
    {
        SceneManager.LoadScene("FungusGameScene");
    }

    public void LoadFungusMenu()
    {
        SceneManager.LoadScene("FungusMainMenuScene");
    }
}
