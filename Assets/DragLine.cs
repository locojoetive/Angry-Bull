using UnityEngine;
using UnityEngine.SceneManagement;

public class DragLine : MonoBehaviour
{
    private LineRenderer line;
    private Transform player;
    private Camera mainCamera;

    void Start()
    {
    }

    void LateUpdate()
    {
        if (!BullMove.speeding && TouchHandler.dragMode == 1) {
            Ray startRay = mainCamera.ScreenPointToRay(mainCamera.WorldToScreenPoint(player.position)),
                endRay = mainCamera.ScreenPointToRay(TouchHandler.dragForceInScreenSpace);
            float rayDistance = mainCamera.nearClipPlane;

            line.SetPosition(0, startRay.GetPoint(rayDistance));
            line.SetPosition(1, endRay.GetPoint(rayDistance));
            
        } else
        {
            line.SetPosition(1, transform.position);
            line.SetPosition(0, transform.position);
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
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
    }
}
