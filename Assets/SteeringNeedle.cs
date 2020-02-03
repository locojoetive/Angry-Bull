using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringNeedle : MonoBehaviour
{
    public float steeringNeedleRotationSpeed = 1F;

    void Update()
    {
        Quaternion newSteeringRotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(new Vector3(0F, 0F, -70F * BullSteer.turnAbout)),
            steeringNeedleRotationSpeed
        );
        transform.rotation = newSteeringRotation;
    }
}
