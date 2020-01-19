using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTiltWithDrag : MonoBehaviour
{
    public float rotationSpeed;
    public float maximumDragLength;
    public Vector3 drag;
    public LineRenderer line;

    void Start()
    {
        line = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        drag = line.GetPosition(1) - line.GetPosition(0);
        if (drag.magnitude > 0.01f)
        {

        }
    }
}
