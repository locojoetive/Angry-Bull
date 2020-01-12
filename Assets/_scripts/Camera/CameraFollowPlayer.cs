using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;
    private Vector3 velocity;
    public float smoothTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTime);
        float posZ = Mathf.SmoothDamp(transform.position.z, player.transform.position.z, ref velocity.z, smoothTime);
        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
