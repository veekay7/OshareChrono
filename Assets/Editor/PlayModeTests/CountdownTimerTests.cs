using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class CountdownTimerTests
{
    [UnityTest]
    public IEnumerator TestSetTimeAndRun()
    {
        Debug.Log("Test Countdown Timer in Playmode: 1 minute countdown!");
        yield return new MonoBehaviourTest<CountdownTimerTestBehaviour>();
    }
}


public class CountdownTimerTestBehaviour : MonoBehaviour, IMonoBehaviourTest, ITimer
{
    public const long k_MillisecondsPerMin = 60000;                              // 1 minute has 60000 milliseconds.
    public const long k_MillisecondsPerSecond = 1000;                            // 1 second has 1000 milliseconds.

    public const long k_DefaultStartTime = (10 * k_MillisecondsPerMin);          // Default start time is at 10 minutes (600 seconds).         // Increment by 1 minute intervals.
    public const long k_CountdownRangeMin = (1 * k_MillisecondsPerMin);          // Countdown timer min time = 1 minute (60 secs).
    public const long k_CountdownRangeMax = (59 * k_MillisecondsPerMin);         // Countdown timer max time = 60 minutes (3600 secs).

    private ETimerState m_state;
    private bool m_canResetTimer;
    private bool m_countdownFinished;
    private long m_startTime;
    private long m_timeRemaining;

    /// <summary>
    /// Returns the current state of the timer.
    /// </summary>
    public ETimerState State { get => m_state; }

    /// <summary>
    /// Returns the start time of the timer.
    /// </summary>
    public TimeSpan StartTime { get => TimeSpan.FromMilliseconds(m_startTime); }

    /// <summary>
    /// The time remaining in the countdown timer.
    /// </summary>
    public TimeSpan TimeRemaining { get => TimeSpan.FromMilliseconds(m_timeRemaining); }

    /// <summary>
    /// Returns the start time in milliseconds.
    /// </summary>
    public long StartTimeMilliseconds { get => m_startTime; }

    /// <summary>
    /// Returns the start time in milliseconds.
    /// </summary>
    public long TimeRemainingMilliseconds { get => m_timeRemaining; }

    public bool IsTestFinished => m_countdownFinished;

    public static long MinutesToMilliseconds(long min) { return min * k_MillisecondsPerMin; }

    public static uint MillisecondsToMinutes(long ms) { return (uint)(ms / k_MillisecondsPerMin); }

    public static float MillisecondsToSeconds(long ms) { return ms / k_MillisecondsPerSecond; }

    public static long SecondsToMilliseconds(float sec) { return (long)(sec * k_MillisecondsPerSecond); }


    private void Awake()
    {
        m_countdownFinished = false;
        SetStartTime(1);
        m_timeRemaining = m_startTime;
        StartTimer();
    }


    private void Update()
    {
        // This flag allows the timer to be reset when the timer state is ticking or paused!
        m_canResetTimer = m_state == ETimerState.Ticking || m_state == ETimerState.Paused;

        // Just continue ticking if we are allowed to tick!
        if (m_state == ETimerState.Ticking)
        {
            m_timeRemaining -= SecondsToMilliseconds(Time.unscaledDeltaTime);
            if (m_timeRemaining <= 0)
            {
                m_timeRemaining = 0;
                m_countdownFinished = true;
                StopTimer();
            }
        }
    }


    public void SetStartTime(int startTimeMins)
    {
        m_startTime = MinutesToMilliseconds(startTimeMins);
        ClampStartTime();
        m_timeRemaining = m_startTime;
    }


    private void ClampStartTime()
    {
        if (m_startTime < k_CountdownRangeMin)
            m_startTime = k_CountdownRangeMax;
        else if (m_startTime > k_CountdownRangeMax)
            m_startTime = k_CountdownRangeMin;
    }


    public void StartTimer()
    {
        if (m_state == ETimerState.Ticking)
            return;

        m_state = ETimerState.Ticking;
    }


    public void StopTimer()
    {
        if (m_state == ETimerState.Stopped)
            return;

        m_state = ETimerState.Stopped;
        ResetTimer();
    }


    public void ResetTimer()
    {
        if (!m_canResetTimer)
            return;

        m_timeRemaining = m_startTime;
    }


    public void PauseTimer()
    {
        if (m_state == ETimerState.Paused)
            return;

        m_state = ETimerState.Paused;
    }


    public void ResetDefaults()
    {
        m_startTime = k_DefaultStartTime;
        m_timeRemaining = m_startTime;
    }
}