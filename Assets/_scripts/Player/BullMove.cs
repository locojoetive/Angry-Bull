/*  The Model:
 *  This module represents the player inside the game.
 *  Therefore it provides functions, which apply changes to the
 *  actual game instance's behaviour in the game world.
 */

using UnityEngine;

public class BullMove : MonoBehaviour
{
    public static Vector3 shootImpulse;
    public static bool speeding;

    public float shootPower;
    public float speedingVelocity = 2F;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        speeding = BullSteer.referenceVelocity.magnitude > speedingVelocity;
        if(TouchHandler.dragReleased)
        {
            TouchHandler.dragReleased = false;
            if(!speeding && TouchHandler.dragMode == 1)
                Shoot();
        }
    }

    public void Shoot()
    {
        TouchHandler.dragReleased = false;
        TouchHandler.dragMode = -1;
        shootImpulse = BullSteer.suggestFacingDirectionToModel();
        rb.AddForce(shootPower * shootImpulse, ForceMode.Impulse);
    }
    
}

