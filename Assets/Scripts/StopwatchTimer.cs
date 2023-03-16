using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchTimer : MonoBehaviour, IStopwatch
{
    [SerializeField] private ChronoInterface m_chronoInterface = null;

    private Stopwatch m_elapsedTimeTimer = new Stopwatch();
    private Stopwatch m_splitTimeTimer = new Stopwatch();
    private List<TimeSpan> m_lapTimes = new List<TimeSpan>();

    public bool IsRunning { get => m_elapsedTimeTimer.IsRunning; }

    public TimeSpan CurrentElapsedTime { get => m_elapsedTimeTimer.Elapsed; }

    public TimeSpan CurrentSplitTime { get => m_splitTimeTimer.Elapsed; }


    private void Awake()
    {
        if (m_chronoInterface != null)
            m_chronoInterface.Stopwatch = this;
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
