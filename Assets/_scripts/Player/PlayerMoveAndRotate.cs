using System;
using UnityEngine;

public class PlayerMoveAndRotate : MonoBehaviour
{
    public static Vector2 playerScreenPosition;
    public static Vector2 lastCapturedMovingTouchPosition;
    public static Vector2 dragForce;
    public static bool moving;
    public static bool dragging;

    private Camera mainCamera;

    public LineRenderer line;

    private Vector3 velocity;
    private Vector3 playerPosition;
    private Vector3 shootImpulse;
    

    private Rigidbody rb;

    private bool active = true;
    private bool shoot;
    public static int fingerId = -1;

    public float shootPower = 10F;
    private float smoothTime = 1F;
    private float minimumVelocity = 0.5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {

        playerPosition = transform.position;
        playerScreenPosition = mainCamera.WorldToScreenPoint(playerPosition);
        moving = rb.velocity.magnitude > minimumVelocity;
        if (active)
        {
            Shoot();
            HandleDragPhase();       
            HandleTouchPhases();
            HandleOrientation();
        }
    }


    private void HandleDragPhase()
    {
        dragging = fingerId != -1;
        if (dragging)
        {
            dragForce = (Vector3) (lastCapturedMovingTouchPosition - playerScreenPosition);
        }
    }

    public bool isSpeeding()
    {
        return rb.velocity.magnitude < minimumVelocity;
    }

    private void HandleTouchPhases()
    {
        foreach (Touch touch in Input.touches) { 
            if (touch.phase == TouchPhase.Began
                && isTouchingPlayer(touch)
                && fingerId == -1
            ) {
                fingerId = touch.fingerId;
            }
            else if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                && fingerId == touch.fingerId
            ) {
                shoot = true;
                fingerId = -1;

                Debug.Log("DragForce: " + dragForce);
                Vector3 projectDragOnBull = new Vector3(dragForce.x / (mainCamera.scaledPixelWidth / 0.5f), 0F, dragForce.y / (mainCamera.scaledPixelHeight));
                projectDragOnBull = transform.localToWorldMatrix * (- projectDragOnBull.normalized);
                Debug.Log("ShootDirection: " + projectDragOnBull);

                
                shootImpulse = shootPower * projectDragOnBull;
            } else if (fingerId == touch.fingerId)
            {
                lastCapturedMovingTouchPosition = touch.position;
            }
        }
    }
    private void HandleOrientation()
    {
        if (dragging)
        {
            transform.forward = new Vector3(dragForce.normalized.x, 0F, dragForce.normalized.y);
        }
        else if (isSpeeding())
        {
            Vector3 targetDirection = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
            transform.LookAt(transform.position + targetDirection);
        }
    }

    private void Shoot()
    {
        if (shoot)
        {
            rb.AddForce(shootImpulse, ForceMode.Impulse);
            Debug.Log("SHOOOOOOOT!! " + shootImpulse);
            shoot = false;
        }
    }

    private Vector3 toWorldPosition(Vector2 position)
    {
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(position) + mainCamera.transform.forward;
        return targetPosition;
    }

    public static bool isTouchingPlayer(Touch touch)
    {
        return (touch.position - playerScreenPosition).magnitude < 100;
    }

    internal void SetActive(bool active)
    {
        this.active = active;
    }

    internal Vector3 getDragForce()
    {
        return dragForce; 
    }
}

