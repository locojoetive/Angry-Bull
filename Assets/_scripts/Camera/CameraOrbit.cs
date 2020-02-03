using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public static Transform pointOfInterest;
    public static Transform[] children;

    public Quaternion startRotation;

    private float cameraDistance = 10F;
    private static Camera mainCamera;
    public float draggingSpeed = 5F;
    public static Vector3 projectedReference;
    public float rearrangingSpeed = 50F,
        movingSpeed = 100F;
    private float startDistanceFromPivot;
    private int controlledChildIndex = 0;
    void Start()
    {
        mainCamera = Camera.main;
        pointOfInterest = GameObject.FindGameObjectWithTag("Player").transform;
        children = GetComponentsInChildren<Transform>();
        startDistanceFromPivot = children[controlledChildIndex].localPosition.z;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (!BullMove.speeding && TouchHandler.dragMode == 1)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                TouchHandler.suggestRotationByEulerAngles(transform.rotation.eulerAngles),
                draggingSpeed
            );
            children[controlledChildIndex].localPosition = Vector3.Slerp(
                children[controlledChildIndex].localPosition,
                new Vector3(0F, 0F, TouchHandler.dragForceInCameraSpace.magnitude * startDistanceFromPivot),
                rearrangingSpeed
            );
        } else if(BullMove.speeding) {
            float velocityReference = BullSteer.referenceVelocity.magnitude / 30F;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                BullSteer.suggestRotationDependingOnSpeed(pointOfInterest.eulerAngles),
                movingSpeed
            );
            children[0].localPosition = Vector3.Slerp(
                children[0].localPosition, 
                new Vector3(0F, 0F, startDistanceFromPivot - 5F * velocityReference), 
                rearrangingSpeed
            );
        } else {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                pointOfInterest.rotation * startRotation,
                movingSpeed
            );
            children[0].localPosition = Vector3.Slerp(
                children[0].localPosition, 
                new Vector3(0F, 0F, startDistanceFromPivot), 
                rearrangingSpeed
            );
        }
        transform.position = Vector3.Lerp(
            transform.position,
            pointOfInterest.position,
            movingSpeed
        );
    }


    public static Vector3 suggestFacingDirectionToModel()
    {
        Ray endRay = mainCamera.ScreenPointToRay((Vector2) mainCamera.WorldToScreenPoint(pointOfInterest.position) + TouchHandler.dragForceInScreenSpace);
        float distance = (pointOfInterest.position.y - mainCamera.transform.position.y) / endRay.direction.y;
        projectedReference = endRay.GetPoint(distance);
        Vector3 normal = Vector3.up;
        Vector3 lookDirection = pointOfInterest.position - projectedReference;
        lookDirection = SimpleMath.projectVectorOnPlane(lookDirection, normal);

        Debug.DrawLine(pointOfInterest.position, pointOfInterest.position + lookDirection, Color.red);

        return lookDirection == Vector3.zero
            ? mainCamera.transform.up
            : lookDirection;
    }

}
