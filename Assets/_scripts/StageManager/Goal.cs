using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public static bool stageCleared = false;
    private void Start()
    {

        stageCleared = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            stageCleared = true;
        }
    }
}
