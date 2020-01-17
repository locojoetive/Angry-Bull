using System;
using UnityEngine;

public class PlayerSteer : MonoBehaviour
{
    private Rigidbody rb;
    private bool active = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (active)
        {
            Vector3 targetDirection = new Vector3(rb.velocity.x, 0F, rb.velocity.z);
            if (targetDirection.magnitude > 0.5f)
            {
                transform.LookAt(transform.position + targetDirection);
            }
        }
    }

    internal void SetActive(bool active)
    {
        this.active = active;
    }
}
