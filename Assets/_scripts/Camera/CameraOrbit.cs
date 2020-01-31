using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Transform pointOfInterest,
        pivot;

    protected Vector3 localRotation = Vector3.zero;
    protected float cameraDistance = 10F;

    public float tiltSensitivity = 4F,
        dragSensitivity = 2F,
        orbitDampening = 10F,
        scrollDampening = 6F,
        moveDampening = 0.02F;
    public bool CameraDisabled = false;

    void Start()
    {
        pointOfInterest = GameObject.FindGameObjectWithTag("Player").transform;
        pivot = transform.parent;
    }

    void LateUpdate()
    {
        transform.parent.position = Vector3.Lerp(transform.parent.position, pointOfInterest.position, moveDampening);
        
        if (PlayerMoveAndRotate.dragging)
        {
            Vector3 normalizedDragForce = new Vector3();

            // TODO: make y-rotation dependant on angle between transform.up and and dragForce
            float alpha = Vector3.SignedAngle(transform.up, PlayerMoveAndRotate.dragForce,transform.forward);
            localRotation.y -= 5F * PlayerMoveAndRotate.dragForce.x / (Camera.main.scaledPixelWidth/2F); 
            localRotation.x = Mathf.Lerp(localRotation.x, 90F * (1F + PlayerMoveAndRotate.dragForce.y / (Camera.main.scaledPixelHeight/2F)), dragSensitivity); 
            localRotation.x = Mathf.Clamp(localRotation.x, 0F, 90F);
            transform.parent.localRotation = Quaternion.Euler(localRotation);
        }
        else if(PlayerMoveAndRotate.moving)
        {
            localRotation.x += pointOfInterest.GetComponent<Rigidbody>().velocity.magnitude * tiltSensitivity;
            localRotation.x = Mathf.Clamp(localRotation.x, 10F, 90F);
            transform.parent.localRotation = Quaternion.Euler(localRotation);
        }
        else
        {
            localRotation.x = 90F;
            transform.parent.localRotation = Quaternion.Euler(localRotation);
        }
        /*
        if (rotating)
        {
            localRotation.y -= PlayerMoveAndRotate.dragForce.normalized.x * dragSensitivity;
        }
        */
    }
}
