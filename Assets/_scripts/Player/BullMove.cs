using UnityEngine;

public class BullMove : MonoBehaviour
{
    public static Vector3 shootImpulse;
    public static Vector2 playerScreenPosition;
    public bool moving;

    private Camera mainCamera;

    private Rigidbody rb;

    private Vector3 playerPosition;
    private Vector3 velocity;

    private bool active = true;

    public float shootPower;
    private float smoothTime = 1F,
        minimumVelocity = 0.5f;

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

        if (moving)
            Time.timeScale = 0.5f;
        else
            Time.timeScale = 1F;
    }

    private void FixedUpdate()
    {
        if (active)
            Shoot();
    }

    private void LateUpdate()
    {
        if (active)
            HandleOrientation();
    }

    public bool isSpeeding()
    {
        return !moving;
    }

    private void HandleOrientation()
    {
        if (TouchHandler.dragging)
        {
            transform.forward = CameraOrbit.getLookDirection();
        }
    }

    private void Shoot()
    {
        if (TouchHandler.shoot)
        {
            shootImpulse = shootPower * transform.forward;
            rb.AddForce(shootImpulse, ForceMode.Impulse);
            Debug.Log("SHOOOOOOOT!! " + shootImpulse);
            TouchHandler.shoot = false;
        }
    }

    private Vector3 toWorldPosition(Vector2 position)
    {
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(position) + mainCamera.transform.forward;
        return targetPosition;
    }


    internal void SetActive(bool active)
    {
        this.active = active;
    }

}

