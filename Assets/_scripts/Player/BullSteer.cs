using System;
using UnityEngine;

public class BullSteer : MonoBehaviour
{
    public static Vector3 referenceVelocity;

    private Rigidbody rb;
    private Vector3 turnToTargetector;
    public static float turnAbout = 0F;
    public float turnFactor = 0F;
    private float smoothTime = 1F;
    private bool active = true;
    public float rotationSpeed;
    public float bullSteeringSensitivity;
    public float draggingCircleSpeed;
    private bool collided;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        referenceVelocity = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
    }

    private void LateUpdate()
    {
        if (!BullMove.speeding && TouchHandler.dragMode == 1)
        {
            transform.forward = CameraOrbit.suggestFacingDirectionToModel();
        }
        else if (BullMove.speeding && (!TouchHandler.dragging || collided))
        {
            //            Debug.Log("Orientate on Speed!");
            collided = false;
            transform.forward = referenceVelocity;
        }
        else if (BullMove.speeding && TouchHandler.dragMode == 2)
        {
  //          Debug.Log("Orientate on steering");
            Steer();
        }
        else turnAbout = 0F;
        
    }

    private void Steer()
    {
        turnAbout = TouchHandler.dragForceInCameraSpace.x;
        Debug.Log(turnAbout);
        Vector3 newEulers = new Vector3(0F, bullSteeringSensitivity*turnAbout, 0F);
        Quaternion newBullRotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(newEulers) * transform.rotation,
            rotationSpeed
        );
        Vector3 newTargetDirection = transform.TransformDirection(new Vector3(bullSteeringSensitivity * turnAbout, 0F, 0F));
        rb.AddForce(newTargetDirection, ForceMode.Force);
    }
    internal static Quaternion suggestRotationDependingOnSpeed(Vector3 currentEulerAngles)
    {
        Vector3 localRotation = currentEulerAngles;
        float movingSpeed = referenceVelocity.magnitude / 30F;
        localRotation.x = 45F * (1F - movingSpeed);
        localRotation.x = Mathf.Clamp(localRotation.x, 45F, 90F);
        localRotation.z = 0F;
        return Quaternion.Euler(localRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Floor")
            collided = true;
    }
}
