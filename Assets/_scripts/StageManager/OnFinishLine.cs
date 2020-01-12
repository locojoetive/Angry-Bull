using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinishLine : MonoBehaviour
{
    bool cleared = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Finished!");
        cleared = true;
    }

    public bool isStageCleared()
    {
        return cleared;
    }
}
