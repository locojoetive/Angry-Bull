using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI,
        steeringUI,
        player;
    private string activeScene;

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
    }

    public void OnPause()
    {
        if (gamePaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        steeringUI.SetActive(false);
        player.GetComponent<PlayerSteer>().SetActive(false);
        player.GetComponent<PlayerMove>().SetActive(false);
        Time.timeScale = 0F;
        gamePaused = true;
    }

    private void Resume ()
    {
        pauseMenuUI.SetActive(false);
        steeringUI.SetActive(true);
        player.GetComponent<PlayerSteer>().SetActive(true);
        player.GetComponent<PlayerMove>().SetActive(true);
        Time.timeScale = 1F;
        gamePaused = false;
    }

    public void TryAgain()
    {
        Resume();
        SceneManager.LoadScene(activeScene);
    }

    public void ExitGame()
    {
        Resume();
        SceneManager.LoadScene("gameMenu");
    }
}
