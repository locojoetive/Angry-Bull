using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject pauseMenu,
        pauseButton,
        clearedMenu,
        failedMenu,
        steering,
        timer;
    private GameObject player;

    public static bool paused = false,
        cleared = false,
        failed = false,
        inGame = true;
    private bool speeding;

    private static int currentStage = 0, lastStage = 1;
    private string activeScene;
    private bool nextStageLoading;

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("StageManager").Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (inGame)
        {
            HandleGameState();
            HandleGameComponents();
        }
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
        speeding = inGame && BullMove.speeding;
    }

    private void HandleGameComponents()
    {
        pauseButton.SetActive(!paused && !cleared && !failed);
        pauseMenu.SetActive(paused);
        clearedMenu.SetActive(cleared);
        failedMenu.SetActive(failed);
        timer.SetActive(inGame);
        steering.SetActive(speeding);
    }


    public void OnPause()
    {
        if (paused) Pause(false);
        else Pause(true);
    }

    private static void Pause(bool pause)
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

    private void ResetModule()
    {
        nextStageLoading = false;
        paused = false;
        cleared = false;
        failed = false;
        timer.GetComponent<Timer>().ResetTime();
    }

    public void OnNextStage()
    {
        if (!nextStageLoading)
        {
            nextStageLoading = true;
            if (currentStage == lastStage) SceneManager.LoadScene("credits");
            else
            {
                currentStage++;
                SceneManager.LoadScene("stage" + currentStage);
            }
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        activeScene = SceneManager.GetActiveScene().name;
        if (activeScene.Contains("stage"))
        {
            OnStageLoaded();
        }
        else
        {
            OnMenuLoaded();
        }
    }

    private void OnMenuLoaded()
    {
        inGame = false;
        ResetModule();
        DeactivateUI();
        Time.timeScale = 1F;
    }

    private void DeactivateUI()
    {
        timer.SetActive(false);
        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);
        clearedMenu.SetActive(false);
        failedMenu.SetActive(false);
    }

    private void OnStageLoaded()
    {
        inGame = true;
        ResetModule();
    }


}
