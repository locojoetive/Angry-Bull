using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStage0 : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("stage0");
    }

    public void OnBackToMenu()
    {
        SceneManager.LoadScene("gameMenu");
    }
}
