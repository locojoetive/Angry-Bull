using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static bool paused = false,
        cleared = false,
        failed = false,
        inGame = true;

    private static int currentStage = 0, lastStage = 1;

    public GameObject pauseMenu,
        pauseButton,
        clearedMenu,
        failedMenu,
        steering,
        timer;

    private string activeScene;
    private bool nextStageLoading;

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        activeScene = SceneManager.GetActiveScene().name;
        if (activeScene.Contains("stage"))
            OnStageLoaded();
        else
            OnMenuLoaded();
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        // Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("StageManager").Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (inGame)
        {
            HandleGameState();
            HandleUIComponents();
        }
    }

    private void HandleGameState()
    {
        cleared = failed && !cleared 
            ? false 
            : Goal.stageCleared;
        failed = cleared 
            ? false 
            : Timer.gameOver;
    }

    private void HandleUIComponents()
    {
        pauseButton.SetActive(inGame && !paused && !cleared && !failed);
        pauseMenu.SetActive(paused);
        clearedMenu.SetActive(cleared);
        failedMenu.SetActive(failed);
        timer.SetActive(inGame);
        steering.SetActive(inGame && BullMove.speeding);
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

    private void OnStageLoaded()
    {
        inGame = true;
        ResetModule();
    }

    private void OnMenuLoaded()
    {
        inGame = false;
        ResetModule();
        DeactivateUI();
    }

    private void ResetModule()
    {
        nextStageLoading = false;
        paused = false;
        cleared = false;
        failed = false;
        timer.GetComponent<Timer>().ResetTime();
    }

    private void DeactivateUI()
    {
        timer.SetActive(false);
        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);
        clearedMenu.SetActive(false);
        failedMenu.SetActive(false);
        steering.SetActive(false);
    }
}
