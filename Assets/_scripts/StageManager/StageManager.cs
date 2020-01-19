using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject pauseMenu,
        pauseButton,
        clearedMenu,
        failedMenu,
        steering,
        player;

    public bool paused = false,
        cleared = false,
        failed = false;

    private static int currentStage = 0, lastStage = 0;
    private string activeScene;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
        paused = false;
        cleared = false;
        failed = false;
    }

    private void Update()
    {
        HandleGameState();
        HandleGameComponents();
    }

    private void HandleGameState()
    {
        Time.timeScale = paused || cleared || failed ? 0F : 1F;
        cleared = failed && !cleared 
            ? false 
            : Goal.stageCleared;
        failed = cleared 
            ? false 
            : Timer.gameOver;
    }

    private void HandleGameComponents()
    {
        steering.SetActive(!paused && !cleared & !failed);
        pauseButton.SetActive(!paused && !cleared && !failed);
        pauseMenu.SetActive(paused);
        clearedMenu.SetActive(cleared);
        failedMenu.SetActive(failed);
    }


    public void OnPause()
    {
        Debug.Log("Sumthin happenin?");
        if (paused) Pause(false);
        else Pause(true);
    }

    private void Pause(bool pause)
    {
        paused = pause;
    }

    public void OnTryAgain()
    {
        SceneManager.LoadScene(activeScene);
    }

    public void OnExitGame()
    {
        SceneManager.LoadScene("gameMenu");
    }

    public void OnNextStage()
    {
        if (currentStage == lastStage) SceneManager.LoadScene("credits");
        else
        {
            currentStage++;
            SceneManager.LoadScene("stage" + currentStage);
        }
    }
}
