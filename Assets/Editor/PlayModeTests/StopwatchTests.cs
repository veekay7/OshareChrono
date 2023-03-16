using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StopwatchTests
{
    [UnityTest]
    public IEnumerator TestStopwatchRun()
    {
        yield return new MonoBehaviourTest<StopwatchTestBehaviour>();
    }
}


public class StopwatchTestBehaviour : MonoBehaviour, IMonoBehaviourTest, IStopwatch
{
    private Stopwatch m_elapsedTimeTimer = new Stopwatch();
    private Stopwatch m_splitTimeTimer = new Stopwatch();
    private List<TimeSpan> m_lapTimes = new List<TimeSpan>();

    private const int k_MaxTestFramesNum = 600;     // 10 seconds!
    private bool m_testFinished = false;
    private int m_frameNum = 0;

    public bool IsRunning { get => m_elapsedTimeTimer.IsRunning; }

    public TimeSpan CurrentElapsedTime { get => m_elapsedTimeTimer.Elapsed; }

    public TimeSpan CurrentSplitTime { get => m_splitTimeTimer.Elapsed; }

    public bool IsTestFinished => m_testFinished;


    private void Start()
    {
        StartStopwatch();
    }


    private void Update()
    {
        // Every odd number frame I record a lap! I expected results is 3.
        if (m_frameNum == 100)
            SplitTime();
        else if (m_frameNum == 300)
            SplitTime();
        else if (m_frameNum == 500)
            SplitTime();

        if (m_frameNum == k_MaxTestFramesNum)
        {
            StopStopwatch();
            m_testFinished = true;
            UnityEngine.Debug.LogFormat("Lap times count: {0}", m_lapTimes.Count);

            for (int i = 0; i < m_lapTimes.Count; i++)
            {
                UnityEngine.Debug.LogFormat("Lap #{0}: {1:00}:{2:00}.{3:00}", 
                    i+1, m_lapTimes[i].Minutes, m_lapTimes[i].Seconds, m_lapTimes[i].Milliseconds);
            }
            return;
        }

        if (!m_testFinished)
            m_frameNum++;
    }

    /// <summary>
    /// Starts or resumes the stopwatch! You can only start the stopwatch if the stopwatch is running!
    /// </summary>
    public void StartStopwatch()
    {
        if (m_elapsedTimeTimer.IsRunning)
            return;

        m_elapsedTimeTimer.Start();
        m_splitTimeTimer.Start();
    }


    /// <summary>
    /// You can only stop the stopwatch if it is running. Stoopping the stopwatch pauses it, does not reset to zero.
    /// </summary>
    public void StopStopwatch()
    {
        if (!m_elapsedTimeTimer.IsRunning)
            return;

        m_elapsedTimeTimer.Stop();
        m_splitTimeTimer.Stop();
    }


    /// <summary>
    /// Resets the stopwatch by clearing the laptimes and stops counting. Elapsed time is reset to 0.
    /// This function only works when the stopwatch has paused/stopped.
    /// </summary>
    public void ResetStopwatch()
    {
        if (m_elapsedTimeTimer.IsRunning)
            return;

        m_lapTimes.Clear();
        m_elapsedTimeTimer.Reset();
        m_splitTimeTimer.Reset();
    }


    /// <summary>
    /// Records a lap time.
    /// </summary>
    public TimeSpan SplitTime()
    {
        // We will only use the split time timer elapsed time!
        var split = m_splitTimeTimer.Elapsed;
        m_lapTimes.Add(split);

        //m_elapsedTimeTimer.Restart();
        m_splitTimeTimer.Restart();     // Only restart the split time timer!

        return split;
    }
}
