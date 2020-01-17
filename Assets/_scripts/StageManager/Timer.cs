using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    public float
        passedTime,
        maxTime;
    public static bool gameOver = false;

    private void Start()
    {

        gameOver = false;
    }

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
