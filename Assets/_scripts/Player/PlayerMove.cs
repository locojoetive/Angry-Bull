using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Camera mainCamera;

    public LineRenderer line;

    private Vector3 playerPosition;
    private Vector3 shootImpulse;
    private Vector3 velocity;
    private Vector3 turnTotargetVector;

    private Vector2 playerScreenPosition;

    private Rigidbody rb;

    public bool steerLeft;
    private bool active = true;

    public int fingerIdMoving = -1, fingerIdSteering = -1;
    private int screenSizeXHalf;

    public float turnFactor,
        shootPower = 10F;
    private float smoothTime = 1F;
    private float turnAbout;


    internal void SetActive(bool active)
    {
        this.active = active;
    }

    private bool moving;
    public RectTransform steering;

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
            screenSizeXHalf = mainCamera.scaledPixelWidth / 2;
            moving = rb.velocity.magnitude > 1F;
            if (fingerIdMoving == -1)
            {
                line.SetPosition(0, playerPosition);
                line.SetPosition(1, playerPosition);
            }
            else
            {
                line.SetPosition(0, playerPosition);
            }
            foreach (Touch touch in Input.touches)
            {
                HandleTouchPhase(touch);
                Steer(touch);
            }
            if (fingerIdSteering == -1)
            {
                Quaternion newSteeringRotation = Quaternion.Slerp(
                    steering.transform.rotation,
                    Quaternion.Euler(new Vector3(0F, 0F, 0F)),
                    0.075f
                );
                steering.transform.rotation = newSteeringRotation;
            }
        }
    }

    private void HandleTouchPhase(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            if (fingerIdMoving == -1 && fingerIdSteering == -1 && isTouchingPlayer(touch))
            {
                fingerIdMoving = touch.fingerId;
            }
            else if (fingerIdMoving == -1 && fingerIdSteering == -1)
            {
                fingerIdSteering = touch.fingerId;
            }
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            if (fingerIdMoving == touch.fingerId)
            {
                line.SetPosition(1, toWorldPosition(touch.position));
            }
        }
        else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            if (fingerIdMoving == touch.fingerId)
            {
                line.SetPosition(1, playerPosition);
                Shoot(touch.position);
                fingerIdMoving = -1;
            }
            else if (fingerIdSteering == touch.fingerId)
            {
                fingerIdSteering = -1;
            }
        }
    }

    private void Steer(Touch touch)
    {
        if (fingerIdSteering == touch.fingerId)
        {
            if (touch.position.x < screenSizeXHalf) turnAbout = -((screenSizeXHalf - touch.position.x) / screenSizeXHalf);
            else turnAbout = ((touch.position.x - screenSizeXHalf) / screenSizeXHalf);
            Vector3 currentDirection = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
            Vector3 targetDirection = SimpleMath.RotateTowards(currentDirection, Vector3.up, turnAbout * turnFactor);
            rb.velocity = targetDirection;
            Quaternion newSteeringRotation = Quaternion.Slerp(
                steering.transform.rotation,
                Quaternion.Euler(new Vector3(0F, 0F, -turnAbout * 90F)),
                0.075f
            );
            steering.transform.rotation = newSteeringRotation;
            Debug.Log(turnAbout);
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

    public bool isTouchingPlayer(Touch touch)
    {
        return (touch.position - playerScreenPosition).magnitude < 100;
    }
}

