using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    float time = 0F,
        reactAfter = 0.5f;

    private void Start()
    {
        time = 0F;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > reactAfter)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    if (activeSceneIsMenu())
                        SceneManager.LoadScene("stage0");
                    else if (activeSceneIsCredits())
                        SceneManager.LoadScene("gameMenu");
                }
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
