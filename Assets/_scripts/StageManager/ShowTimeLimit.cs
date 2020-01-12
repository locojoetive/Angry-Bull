using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTimeLimit : MonoBehaviour
{
    public Text timeText;
    public float
        passedTime,
        maxTime;
    private bool gameOver;

    void Update()
    {
        passedTime += Time.deltaTime;
        if (maxTime < passedTime)
        {
            gameOver = true;
            timeText.text = maxTime + " / " + maxTime;
        } else
        {
            timeText.text = (int) passedTime + " / " + maxTime;
        }
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
