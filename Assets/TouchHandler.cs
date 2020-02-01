using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    public static int fingerId = -1;
    public static Vector2 dragForceInScreenSpace;
    public static Vector2 dragForceInCameraSpace;
    public static bool shoot;
    public static bool dragging;

    private Vector2 lastCapturedTouchStartPosition = Vector2.zero;
    private Vector2 lastCapturedTouchPosition = Vector2.zero;
    private Camera mainCamera;
    private Vector3 shootImpulse;
    private float shootPower;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleTouchPhases();
        HandleDrag();
    }


    private void HandleTouchPhases()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began
                && fingerId == -1
            )
            {
                lastCapturedTouchStartPosition = touch.position;
                fingerId = touch.fingerId;
            }
            else if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                && fingerId == touch.fingerId
            )
            {
                shoot = true;
                fingerId = -1;
                Debug.Log("DragForce: " + dragForceInScreenSpace);
                Debug.Log("ShootDirection: " + dragForceInCameraSpace);
            }
            else if (fingerId == touch.fingerId)
            {
                lastCapturedTouchPosition = touch.position;
            }
        }
    }

    public static bool isTouchingPlayer(Touch touch)
    {
        return (touch.position - BullMove.playerScreenPosition).magnitude < 100;
    }

    internal Vector3 getDragForce()
    {
        return dragForceInScreenSpace;
    }

    private void HandleDrag()
    {
        dragging = fingerId != -1;
        if (dragging)
        {
            dragForceInScreenSpace = (Vector3) (lastCapturedTouchPosition - lastCapturedTouchStartPosition);
            dragForceInCameraSpace = dragForceInScreenSpace;
            dragForceInCameraSpace.x = dragForceInCameraSpace.x / (0.5f * mainCamera.scaledPixelWidth);
            dragForceInCameraSpace.y = dragForceInCameraSpace.y / (0.5f * mainCamera.scaledPixelHeight);
        }
    }

}
