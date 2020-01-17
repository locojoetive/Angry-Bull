using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuFromCredits : MonoBehaviour
{
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            SceneManager.LoadScene("gameMenu");
        }
        
    }
}
