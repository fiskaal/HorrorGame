using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Time.timeScale == 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
