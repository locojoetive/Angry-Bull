using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTiltWithDrag : MonoBehaviour
{
    public float rotationSpeed;
    public float maximumDragLength;
    public Vector3 drag;
    public LineRenderer line;
    private PlayerMove player;
    public float smoothTime,
        sphereRadius;
    private Vector3 velocity,
        targetPositon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        line = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<LineRenderer>();
        sphereRadius = transform.position.y;
    }


    void Update()
    {
        if (player.dragging)
        {
            drag = line.GetPosition(1) - line.GetPosition(0);
            targetPositon = transform.position + drag;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPositon,
                ref velocity,
                smoothTime
            );
            transform.LookAt(player.transform);
        } else if (!player.moving)
        {
            Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Quaternion newRotation = Quaternion.Euler(newEuler);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTime);
            targetPositon = player.transform.position;
        } else
        {
            targetPositon = player.transform.position;
        }
    }

    public Vector3 getTargetPosition()
    {
        return targetPositon;
    }
}
