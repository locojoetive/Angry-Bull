using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Vector3 velocity;
    public float smoothTime;
    private CameraTiltWithDrag tiltWithDrag;
    private Transform player;

    private void Start()
    {
        tiltWithDrag = GetComponent<CameraTiltWithDrag>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Vector3 targetPosition = player.position;
        float posX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, smoothTime);
        float posZ = Mathf.SmoothDamp(transform.position.z, targetPosition.z, ref velocity.z, smoothTime);
        posX = float.IsNaN(posX) ? 0F : posX;
        posZ = float.IsNaN(posZ) ? 0F : posZ;
        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
