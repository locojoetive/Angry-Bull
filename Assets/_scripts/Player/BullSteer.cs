using UnityEngine;

public class BullSteer : MonoBehaviour
{
    public static Vector3 referenceVelocity;
    public static float turnAbout = 0F;
    private new static Transform transform;

    public float bullSteeringSensitivity;

    private Rigidbody rb;
    private bool collided = false;

    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        referenceVelocity = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
    }

    void FixedUpdate()
    {
        if (!BullMove.speeding && TouchHandler.dragMode == 1)
        {
            transform.forward = suggestFacingDirectionToModel();
        }
        else if (BullMove.speeding)
        {
            Steer();
            transform.forward = referenceVelocity;
        }
        else
        {
            turnAbout = 0F;
        }
    }

    private void Steer()
    {
        turnAbout = TouchHandler.getSteeringInput();
        Vector3 newTargetDirection = SimpleMath.RotateTowards(rb.velocity, transform.up, bullSteeringSensitivity * turnAbout);
        rb.velocity = newTargetDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Floor")
            collided = true;
    }

    public static Vector3 suggestFacingDirectionToModel()
    {
        Camera mainCamera = Camera.main;
        Ray endRay = mainCamera.ScreenPointToRay((Vector2)mainCamera.WorldToScreenPoint(transform.position) + TouchHandler.dragForceInScreenSpace);
        float distance = (transform.position.y - mainCamera.transform.position.y) / endRay.direction.y;
        Vector3 projectedReference = endRay.GetPoint(distance);
        Vector3 normal = Vector3.up;
        Vector3 lookDirection = transform.position - projectedReference;
        lookDirection = SimpleMath.projectVectorOnPlane(lookDirection, normal);
        Debug.DrawLine(transform.position, transform.position + lookDirection, Color.red);
        return lookDirection == Vector3.zero
            ? mainCamera.transform.up
            : lookDirection;
    }

    public static Quaternion suggestRotationDependingOnSpeed(Vector3 currentEulerAngles)
    {
        Vector3 localRotation = currentEulerAngles;
        float movingSpeed = referenceVelocity.magnitude / 30F;
        localRotation.x = 45F * (1F - movingSpeed);
        localRotation.x = Mathf.Clamp(localRotation.x, 45F, 90F);
        localRotation.z = 0F;
        return Quaternion.Euler(localRotation);
    }
}
