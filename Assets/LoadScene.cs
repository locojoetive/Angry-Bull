using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                if (activeSceneIsMenu())
                    SceneManager.LoadScene("stage0");
                if (activeSceneIsCredits())
                    SceneManager.LoadScene("gameMenu");
            }
        }
    }

    private bool activeSceneIsCredits()
    {
        return SceneManager.GetActiveScene().name == "credits";
    }

    private bool activeSceneIsMenu()
    {
        return SceneManager.GetActiveScene().name == "gameMenu";
    }
}
