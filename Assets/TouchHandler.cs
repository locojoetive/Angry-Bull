/*  The Controller:
 *  This Module shall handle the inputs and provide the player's current intende motion,
 *  to other game components.
 *  It has to provide functions, which alternate the model (bull).
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchHandler : MonoBehaviour
{
    public static Vector3 tilt;
    public static Vector2 dragForceInScreenSpace;
    public static Vector2 dragForceInCameraSpace;
    public static int fingerId = -1;
    public static int dragMode = 0;
    public static bool dragReleased = false;
    public static bool dragging = false;
    public static bool tiltControl;

    public static float dragSensitivity = 50F;

    private Vector2 lastCapturedTouchStartPosition = Vector2.zero;
    private Vector2 lastCapturedTouchPosition = Vector2.zero;
    private Camera mainCamera;
    private float time = 0F;
    private float reactAfter = 0.1f;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Debug.Log(tiltControl);
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

    internal static float getSteeringInput()
    {
        if (dragging && dragMode == 2)
        {
            return dragForceInCameraSpace.x;
        }
        else if (tiltControl)
        {
            return tilt.x;
        }
        else
        {
            return 0F;
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
            bool buttonInvoked = invokeUIButtonByTouch(touch);
            if (!buttonInvoked)
            {
                if (touch.phase == TouchPhase.Began
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
    }

    private bool invokeUIButtonByTouch(Touch touch)
    {
        if(touch.phase == TouchPhase.Began)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            int buttonCount = 0;
            results.ForEach(delegate(RaycastResult result) {
                if (result.gameObject.GetComponent<Button>())
                {
                    Button button = result.gameObject.GetComponent<Button>();
                    Debug.Log(button.name);
                    button.onClick.Invoke();
                    buttonCount++;
                } else if (result.gameObject.transform.parent.parent.GetComponent<Toggle>())
                {
                    Debug.Log(result.gameObject.transform.parent.parent.name);
                    Toggle toggle = result.gameObject.transform.parent.parent.GetComponent<Toggle>();
                    toggle.onValueChanged.Invoke(!toggle.isOn);
                    buttonCount++;
                }

            });
            return buttonCount > 0;
        }
        return false;
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
