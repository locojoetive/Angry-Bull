using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public static Transform pointOfInterest;
    public static Transform[] children;

    public Quaternion startRotation;
    public float draggingSpeed = 5F,
        rearrangingSpeed = 50F,
        movingSpeed = 100F;

    private static Camera mainCamera;
    private float cameraDistance = 10F;
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
                suggestRotationByEulerAngles(transform.rotation.eulerAngles),
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

    private Quaternion suggestRotationByEulerAngles(Vector3 currentEulerAngles)
    {
        float dragSensitivity = TouchHandler.dragSensitivity;
        Vector2 dragForceInCameraSpace = TouchHandler.dragForceInCameraSpace;
        Vector3 localRotation = currentEulerAngles;
        float dragForceMagnitude = Mathf.Abs(dragForceInCameraSpace.x) > 0.2f
            ? TouchHandler.dragForceInCameraSpace.x - 0.2f * Mathf.Sign(dragForceInCameraSpace.x)
            : 0F;
        localRotation.y -= dragSensitivity * dragForceMagnitude;
        localRotation.x = Mathf.Lerp(localRotation.x, 90F * (1F + dragForceInCameraSpace.y), dragSensitivity);
        localRotation.x = Mathf.Clamp(localRotation.x, 0F, 90F);
        localRotation.z = 0F;
        return Quaternion.Euler(localRotation);
    }
    

}
