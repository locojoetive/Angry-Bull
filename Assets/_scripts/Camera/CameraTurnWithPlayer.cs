using UnityEngine;

public class CameraTurnWithPlayer : MonoBehaviour
{
    private PlayerMove player;
    private float playerRotationY,
        tiltRotation,
        velocity;
    public float smoothTimeMoving,
        smoothTimeDragging,
        smoothTimeDefault;
    public float minimumAngle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }


    void Update()
    {

        Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, player.transform.eulerAngles.y, transform.rotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(newEuler);
        if (player.moving)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTimeMoving);
        } else if (!player.dragging)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTimeDefault);
        }
        else if (player.dragging && Mathf.Abs(Vector3.SignedAngle(transform.up, player.transform.forward, Vector3.up)) > minimumAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTimeDragging);
        }
    }
}
