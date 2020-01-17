using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltWithPlayer : MonoBehaviour
{

    private GameObject player;
    private LineRenderer draggedLineRenderer;

    private float playerSpeed,
        tiltRotation,
        velocity;
    public float smoothTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        draggedLineRenderer = player.GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        playerSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
        tiltRotation = (draggedLineRenderer.GetPosition(1) - draggedLineRenderer.GetPosition(0)).magnitude;

        if (tiltRotation == 0F)
        {

        }


        Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(newEuler);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTime);
    }
}
