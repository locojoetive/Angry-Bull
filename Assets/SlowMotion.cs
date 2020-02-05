using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float slowMotionTimeScale;
    public float duration;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        bool gamePaused = (StageManager.paused || StageManager.cleared || StageManager.failed);
        if (gamePaused && Time.timeScale != 0F)
        {
            interpolateTimeScale(0F);
        }
        else if (!gamePaused && BullMove.speeding)
        {
            if (Time.timeScale != slowMotionTimeScale)
                interpolateTimeScale(slowMotionTimeScale);
        }
        else if (!gamePaused && Time.timeScale != 1F)
        {
            interpolateTimeScale(1F);
        }
    }

    private void interpolateTimeScale(float to)
    {
        float t = (Time.time - startTime) / duration;
        float timeScale = Mathf.SmoothStep(Time.timeScale, to, t);
        Time.timeScale = timeScale;
    }
}
