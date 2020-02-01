using System;
using UnityEngine;

public class PlayerSteer : MonoBehaviour
{
    private Camera mainCamera;

    private Rigidbody rb;
    public RectTransform steeringNeedle;
    public GameObject steering;
    private Vector3 turnToTargetector;

    private int screenSizeXHalf;
    public static int fingerId = -1;

    public float turnFactor = 0F;
    private float smoothTime = 1F,
        turnAbout = 0F;

    private bool active = true;
    public float minimumSpeed;

    private BullMove move;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        move = GetComponent<BullMove>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (active)
        {
            screenSizeXHalf = mainCamera.scaledPixelWidth / 2;
            foreach (Touch touch in Input.touches)
            {
                HandleTouchPhase(touch);
            }
            Steer();
            HandleSteeringComponents();
        }
    }

    private void HandleSteeringComponents()
    {
        if (rb.velocity.magnitude < minimumSpeed)
            steering.SetActive(false);
        else
            steering.SetActive(true);
    }

    private void HandleTouchPhase(Touch touch)
    {
        if (touch.phase == TouchPhase.Began
            && !TouchHandler.dragging
            && fingerId == -1
            && !TouchHandler.isTouchingPlayer(touch)
        ) {
            fingerId = touch.fingerId;
        }
        else if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && fingerId == touch.fingerId
        ) {
            fingerId = -1;
        }
    }


    private void Steer()
    {
        if (fingerId != -1)
        {
            Touch touch = getSteeringTouch();
            if (touch.position.x < screenSizeXHalf) turnAbout = -((screenSizeXHalf - touch.position.x) / screenSizeXHalf);
            else turnAbout = ((touch.position.x - screenSizeXHalf) / screenSizeXHalf);
            
            //Vector3 currentDirection = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
            //Vector3 targetDirection = SimpleMath.RotateTowards(currentDirection, Vector3.up, turnAbout * turnFactor);
            //rb.velocity = targetDirection;

            Quaternion newSteeringRotation = Quaternion.Slerp(
                steeringNeedle.transform.rotation,
                Quaternion.Euler(new Vector3(0F, 0F, -turnAbout * 90F)),
                0.075f
            );
            steeringNeedle.transform.rotation = newSteeringRotation;
        }
        else
        {
            Quaternion newSteeringRotation = Quaternion.Slerp(
                steeringNeedle.transform.rotation,
                Quaternion.Euler(new Vector3(0F, 0F, 0F)),
                0.075f
            );
            steeringNeedle.transform.rotation = newSteeringRotation;
        }
    }


    internal void SetActive(bool active)
    {
        this.active = active;
    }

    internal Touch getSteeringTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerId)
                return touch;
        }
        throw new Exception();
    }
}
