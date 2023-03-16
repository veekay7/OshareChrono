using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine.TestTools;

public class StopwatchTests
{
    StopwatchTimerLogic logic = new StopwatchTimerLogic();


    [UnityTest]
    public IEnumerator StopwatchOperation()
    {
        logic.StartStopwatch();
        Assert.IsTrue(logic.IsRunning);

        // Skip for 3600 frames
        int seconds = 60;
        for (int i = 0; i < 60 * seconds; i++)
        {
            yield return null;
        }
        UnityEngine.Debug.LogFormat("Elapsed Time: {0}", logic.CurrentElapsedTime.Seconds);

        // Skip for 3600 frames
        for (int i = 0; i < 60 * seconds; i++)
        {
            yield return null;
        }
        UnityEngine.Debug.LogFormat("Current Elapsed Time: {0}", logic.CurrentElapsedTime.Seconds);

        var splitTime = logic.SplitTime();
        UnityEngine.Debug.LogFormat("Captured Split Time: {0}", splitTime.Seconds);
        UnityEngine.Debug.LogFormat("Current Split Time: {0}", logic.CurrentSplitTime.Seconds);

        logic.StopStopwatch();
        Assert.IsFalse(logic.IsRunning);

        logic.ResetStopwatch();
        Assert.IsFalse(logic.IsRunning);
        UnityEngine.Debug.LogFormat("Current Elapsed Time: {0}", logic.CurrentElapsedTime.Seconds);
        UnityEngine.Debug.LogFormat("Current Split Time: {0}", logic.CurrentSplitTime.Seconds);
    }
}


public class StopwatchTimerLogic : IStopwatch
{
    private Stopwatch m_elapsedTimeTimer = new Stopwatch();
    private Stopwatch m_splitTimeTimer = new Stopwatch();

    public bool IsRunning { get => m_elapsedTimeTimer.IsRunning; }

    public TimeSpan CurrentElapsedTime { get => m_elapsedTimeTimer.Elapsed; }

    public TimeSpan CurrentSplitTime { get => m_splitTimeTimer.Elapsed; }


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


    public void ResetStopwatch()
    {
        if (m_elapsedTimeTimer.IsRunning)
            return;

        m_elapsedTimeTimer.Reset();
        m_splitTimeTimer.Reset();
    }


    public TimeSpan SplitTime()
    {
        // We will only use the split time timer elapsed time!
        var split = m_splitTimeTimer.Elapsed;

        //m_elapsedTimeTimer.Restart();
        m_splitTimeTimer.Restart();     // Only restart the split time timer!

        return split;
    }
}
