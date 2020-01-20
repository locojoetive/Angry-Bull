using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTiltWithPlayer : MonoBehaviour
{
    private PlayerMove player;
    public float smoothTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (player.moving)
        {

        }
    }
}
