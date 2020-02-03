/*  The Controller:
 *  This Module shall handle the inputs and provide the player's current intende motion,
 *  to other game components.
 *  It has to provide functions, which alternate the model (bull).
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TouchHandler : MonoBehaviour
{
    public static int fingerId = -1;
    public static Vector2 dragForceInScreenSpace;
    public static Vector2 dragForceInCameraSpace;
    public static bool dragReleased = false;
    public static bool dragging = false;
    public static Vector3 tilt;

    private Vector2 lastCapturedTouchStartPosition = Vector2.zero;
    private Vector2 lastCapturedTouchPosition = Vector2.zero;
    private Camera mainCamera;
    public static float dragSensitivity = 5F;
    private float time = 0F;
    private float reactAfter = 0.25f;

    public static int dragMode = 0;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(StageManager.inGame)
        {
            if (time > reactAfter)
            {
                HandleTouchPhases();
                HandleTilt();
                HandleDrag();
            } else
            {
                time += Time.deltaTime;
            }
        }
    }

    private void HandleTilt()
    {
        tilt = Input.acceleration;
    }

    private void HandleTouchPhases()
    {
        foreach (Touch touch in Input.touches)
        {
            if (invokeUIButtonOnScreenPosition(touch.position))
            {

            } else if (touch.phase == TouchPhase.Began
                && fingerId == -1
            ) {
                lastCapturedTouchStartPosition = touch.position;
                fingerId = touch.fingerId;
                dragMode = BullMove.speeding ? 2 : 1;
            } else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    && fingerId == touch.fingerId
            ) {
                dragReleased = true;
                fingerId = -1;
            } else if (fingerId == touch.fingerId) {
                lastCapturedTouchPosition = touch.position;
            }
        }
    }

    private bool invokeUIButtonOnScreenPosition(Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        int buttonCount = 0;
        results.ForEach(delegate(RaycastResult result) {
            if (result.gameObject.GetComponent<Button>())
            {
                Button button = result.gameObject.GetComponent<Button>();
                button.onClick.Invoke();
                buttonCount++;
            }
            
        });
        return buttonCount > 0;
    }

    internal static Quaternion suggestRotationByEulerAngles(Vector3 currentRotation)
    {
        Vector3 localRotation = currentRotation;
        float dragForceMagnitude = Mathf.Abs(dragForceInCameraSpace.x) > 0.2f
            ? dragForceInCameraSpace.x - 0.2f * Mathf.Sign(dragForceInCameraSpace.x)
            : 0F;
        localRotation.y -= 10F * dragSensitivity * dragForceMagnitude;
        localRotation.x = Mathf.Lerp(localRotation.x, 90F * (1F + dragForceInCameraSpace.y), dragSensitivity);
        localRotation.x = Mathf.Clamp(localRotation.x, 0F, 90F);
        localRotation.z = 0F;
        return Quaternion.Euler(localRotation);
    }

    private void HandleDrag()
    {
        dragging = fingerId != -1;
        if (dragging && mainCamera != null)
        {
            dragForceInScreenSpace = lastCapturedTouchPosition - lastCapturedTouchStartPosition;
            dragForceInCameraSpace = new Vector2(
                dragForceInScreenSpace.x / (0.5f * mainCamera.scaledPixelWidth),
                dragForceInScreenSpace.y / (0.5f * mainCamera.scaledPixelHeight)
            );
        } else if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }


}
