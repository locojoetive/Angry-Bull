using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurnWithPlayer : MonoBehaviour
{
    private GameObject player;
    private float playerRotationY,
        tiltRotation,
        velocity;
    public float smoothTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, player.transform.eulerAngles.y, transform.rotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(newEuler);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, smoothTime);
    }
}
