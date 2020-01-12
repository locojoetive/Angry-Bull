using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    ShowTimeLimit time;
    OnFinishLine goal;

    private void Start()
    {
        foreach(GameObject entity in GameObject.FindGameObjectsWithTag("Finish"))
        {
            if (entity.GetComponent<ShowTimeLimit>() != null)
            {
                time = entity.GetComponent<ShowTimeLimit>();
            } else if (entity.GetComponent<OnFinishLine>() != null)
            {
                goal = entity.GetComponent<OnFinishLine>();
            }
        }
    }

    private void Update()
    {
        if (goal.isStageCleared())
        {
            Debug.Log("Thank you for playing our game");
        } else if (time.isGameOver())
        {
            Debug.Log("Time ran out! Wanna try again?");
        }
    }
}
