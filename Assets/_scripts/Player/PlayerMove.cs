using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Camera mainCamera;

    public LineRenderer line;

    private Vector3 playerPosition;
    private Vector3 shootImpulse;
    private Vector3 velocity;
    
    public static Vector2 playerScreenPosition;

    private Rigidbody rb;

    private bool active = true;
    public bool moving = false;

    public static int fingerId = -1;

    public float shootPower = 10F;
    private float smoothTime = 1F;
    internal bool dragging;
    private float dragForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        line.SetPosition(0, playerPosition);
        line.SetPosition(1, playerPosition);
    }

    void Update()
    {
        if (active)
        {
            playerPosition = transform.position;
            playerScreenPosition = mainCamera.WorldToScreenPoint(playerPosition);
            moving = rb.velocity.magnitude > 1F;
            dragging = (line.GetPosition(0) - line.GetPosition(1)).magnitude > 0F;


            HandleDragLine();
            HandleOrientation();

            foreach (Touch touch in Input.touches)
                HandleTouchPhase(touch);
        }
    }


    private void HandleDragLine()
    {
        if (fingerId == -1)
        {
            line.SetPosition(0, playerPosition);
            line.SetPosition(1, playerPosition);
        }
        else
        {
            line.SetPosition(0, playerPosition);
            dragForce = (playerScreenPosition - getMovingTouch().position).magnitude / (
                Camera.main.scaledPixelHeight > Camera.main.scaledPixelWidth 
                        ? Camera.main.scaledPixelHeight
                        : Camera.main.scaledPixelWidth
            );
            Debug.Log(dragForce);
            Vector3 newStartPosition = dragForce * (toWorldPosition(getMovingTouch().position) - playerPosition);
            newStartPosition = new Vector3(newStartPosition.x, playerPosition.y, newStartPosition.z);
            line.SetPosition(1, newStartPosition);
        }
    }

    private void HandleOrientation()
    {
        Vector3 lineLength = new Vector3 (line.GetPosition(0).x, 0F, line.GetPosition(0).z) 
            - new Vector3(line.GetPosition(1).x, 0F, line.GetPosition(1).z);
        float lineAngle = Vector3.SignedAngle(transform.forward, lineLength, Vector3.up);
        Vector3 targetDirection = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
        targetDirection = targetDirection.magnitude > 0.5f 
            ? targetDirection
            : lineLength.magnitude > 0.1f
                ? lineLength
                : transform.forward;
        transform.LookAt(transform.position + targetDirection);
    }

    private void HandleTouchPhase(Touch touch)
    {
        if (touch.phase == TouchPhase.Began
            && isTouchingPlayer(touch)
            && fingerId == -1
        ) {
            fingerId = touch.fingerId;
        }
        else if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            && fingerId == touch.fingerId
        ) {
            Shoot(touch.position);
            fingerId = -1;
        }
    }

    
    private void Shoot(Vector2 position)
    {
        shootImpulse = playerPosition - toWorldPosition(position);
        rb.AddForce(shootPower * shootImpulse, ForceMode.Impulse);
    }

    private Vector3 toWorldPosition(Vector2 position)
    {
        return mainCamera.ScreenToWorldPoint((Vector3) position + new Vector3(0F, 0F, mainCamera.transform.position.y));
    }

    public static bool isTouchingPlayer(Touch touch)
    {
        return (touch.position - playerScreenPosition).magnitude < 100;
    }

    internal void SetActive(bool active)
    {
        this.active = active;
    }

    internal Touch getMovingTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
                return touch;
        }
        throw new Exception();
    }
    internal float getDragForce()
    {
        return dragForce; 
    }
}

