using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public static Transform pointOfInterest,
        pivot;
    protected Vector3 localRotation = Vector3.zero;
    protected float cameraDistance = 10F;
    private static Camera mainCamera;
    public float tiltSensitivity = 4F,
        dragSensitivity = 2F,
        orbitDampening = 10F,
        scrollDampening = 6F,
        moveDampening = 0.02F,
        rotationSpeed = 5F;
    public static Vector3 projectedReference;
    public float rearrangingSpeed = 50F;

    void Start()
    {
        mainCamera = Camera.main;
        pointOfInterest = GameObject.FindGameObjectWithTag("Player").transform;
        pivot = transform.parent;
    }

    void LateUpdate()
    {
        transform.parent.position = Vector3.Lerp(transform.parent.position, pointOfInterest.position, moveDampening);
        
        if (TouchHandler.dragging)
        {
            localRotation.y -= rotationSpeed * TouchHandler.dragForceInCameraSpace.x;
            localRotation.x = Mathf.Lerp(localRotation.x, 90F * (1F + TouchHandler.dragForceInCameraSpace.y), dragSensitivity);
            localRotation.x = Mathf.Clamp(localRotation.x, 0F, 90F);
            transform.parent.localRotation = Quaternion.RotateTowards(
                transform.parent.localRotation,
                Quaternion.Euler(localRotation),
                rearrangingSpeed * Time.deltaTime
            );
        }
        /*else if(BullMove.moving)
        {
        }*/
        else
        {
            localRotation.x = 90F;
            transform.parent.localRotation = Quaternion.RotateTowards(
                transform.parent.localRotation, 
                Quaternion.Euler(localRotation),
                rearrangingSpeed * Time.deltaTime
            );
        }
    }

    public static Vector3 getLookDirection()
    {
        Vector3 normal = Vector3.up;
        Vector3 lookDirection;

        if (TouchHandler.dragging)
        {
            Ray startRay = mainCamera.ScreenPointToRay(BullMove.playerScreenPosition),
                endRay = mainCamera.ScreenPointToRay(BullMove.playerScreenPosition + TouchHandler.dragForceInScreenSpace);
            float distance = (pointOfInterest.position.y - mainCamera.transform.position.y) / endRay.direction.y;
            projectedReference = endRay.GetPoint(distance);
            lookDirection = pointOfInterest.position - projectedReference;
        } else
        {
            lookDirection = mainCamera.transform.forward;
        }
        lookDirection = SimpleMath.projectVectorOnPlane(lookDirection, normal);

        Debug.Log(lookDirection);
        Debug.DrawLine(pointOfInterest.position, pointOfInterest.position + lookDirection, Color.red);

        return lookDirection == Vector3.zero
            ? mainCamera.transform.up 
            : lookDirection;
    }
}
