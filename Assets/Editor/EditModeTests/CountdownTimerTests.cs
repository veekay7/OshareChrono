using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CountdownTimerTests
{
    CountdownTimerLogic logic = new CountdownTimerLogic();


    [Test]
    [TestCase(60000)]
    public void MillisecondsToMinutesConversion(int ms)
    {
        var mins = CountdownTimerLogic.MillisecondsToMinutes(ms);
        Assert.That(mins, Is.EqualTo(1));
    }


    [Test]
    [TestCase(1)]
    public void MinutesToMillisecondConversion(int mins)
    {
        var ms = CountdownTimerLogic.MinutesToMilliseconds(mins);
        Assert.That(ms, Is.EqualTo(60000));
    }


    [Test]
    [TestCase(1000)]
    public void MillisecondsToSecondsConversion(int ms)
    {
        var secs = CountdownTimerLogic.MillisecondsToSeconds(ms);
        Assert.That(secs, Is.EqualTo(1));
    }


    [Test]
    [TestCase(20)]
    public void SecondsToMillisecondsConversion(int secs)
    {
        var ms = CountdownTimerLogic.SecondsToMilliseconds(secs);
        Assert.That(ms, Is.EqualTo(20000));
    }


    [Test]
    [TestCase(3, 3)]
    [TestCase(-1, 59)]
    [TestCase(62, 1)]
    public void SetStartTime(int newStartTimeMins, int expectedResultMins)
    {
        long newStartTimeMs = CountdownTimerLogic.MinutesToMilliseconds(newStartTimeMins);
        logic.SetStartTime(newStartTimeMs);   // Set in milliseconds
        Assert.That(logic.StartTime.Minutes, Is.EqualTo(expectedResultMins));
    }


    [UnityTest]
    public IEnumerator Co_TestCountdown()
    {
        Debug.LogFormat("Starting timer with 5 minute start time");

        Assert.That(logic.State, Is.EqualTo(CountdownTimerLogic.ETimerState.Stopped));

        int secs;
        logic.SetStartTime(CountdownTimerLogic.MinutesToMilliseconds(5));
        logic.StartTimer();

        Assert.That(logic.State, Is.EqualTo(CountdownTimerLogic.ETimerState.Ticking));

        // After 60 seconds, let's check the remaining time.
        secs = 60;
        for (int i = 0; i < 60 * secs; i++)
        {
            logic.Tick();
            yield return null;
        }

        Debug.LogFormat("Start from stopped state after 60 secs: {0:00}:{1:00}", logic.TimeRemaining.Minutes, logic.TimeRemaining.Seconds);

        // Pause the timer.
        logic.PauseTimer();
        Assert.That(logic.State, Is.EqualTo(CountdownTimerLogic.ETimerState.Paused));

        Debug.LogFormat("When timer is paused: {0:00}:{1:00}", logic.TimeRemaining.Minutes, logic.TimeRemaining.Seconds);

        logic.StartTimer();
        Assert.That(logic.State, Is.EqualTo(CountdownTimerLogic.ETimerState.Ticking));

        // Tick for 120 seconds!
        secs = 120;
        for (int i = 0; i < 60 * secs; i++)
        {
            logic.Tick();
            yield return null;
        }

        Debug.LogFormat("After continuing from pausing: {0:00}:{1:00}", logic.TimeRemaining.Minutes, logic.TimeRemaining.Seconds);

        // Stop the timer.
        logic.StopTimer();
        Assert.That(logic.State, Is.EqualTo(CountdownTimerLogic.ETimerState.Stopped));
        Debug.LogFormat("End of test: {0:00}:{1:00}", logic.TimeRemaining.Minutes, logic.TimeRemaining.Seconds);

        Assert.That(logic.TimeRemaining.Minutes, Is.EqualTo(5));
    }
}


public class CountdownTimerLogic : ITimer
{
    public enum ETimerState { Stopped, Ticking, Paused }

    public const long k_MillisecondsPerMin = 60000;                              // 1 minute has 60000 milliseconds.
    public const long k_MillisecondsPerSecond = 1000;                            // 1 second has 1000 milliseconds.

    public const long k_DefaultStartTime = (10 * k_MillisecondsPerMin);          // Default start time is at 10 minutes (600 seconds).
    public const long k_IncrementInterval = (1 * k_MillisecondsPerMin);          // Increment by 1 minute intervals.
    public const long k_CountdownRangeMin = (1 * k_MillisecondsPerMin);          // Countdown timer min time = 1 minute (60 secs).
    public const long k_CountdownRangeMax = (59 * k_MillisecondsPerMin);         // Countdown timer max time = 60 minutes (3600 secs).

    private ETimerState m_state;
    private bool m_canResetTimer;
    private long m_startTime;
    private long m_timeRemaining;

    public Action OnTimerFinishedEvent;

    public ETimerState State { get => m_state; }

    public TimeSpan StartTime { get => TimeSpan.FromMilliseconds(m_startTime); }

    public TimeSpan TimeRemaining { get => TimeSpan.FromMilliseconds(m_timeRemaining); }

    public long StartTimeMilliseconds { get => m_startTime; }

    public long TimeRemainingMilliseconds { get => m_timeRemaining; }


    public static long MinutesToMilliseconds(long min) { return min * k_MillisecondsPerMin; }

    public static uint MillisecondsToMinutes(long ms) { return (uint)(ms / k_MillisecondsPerMin); }

    public static float MillisecondsToSeconds(long ms) { return ms / k_MillisecondsPerSecond; }

    public static long SecondsToMilliseconds(float sec) { return (long)(sec * k_MillisecondsPerSecond); }

    public CountdownTimerLogic()
    {
        m_startTime = k_DefaultStartTime;
        m_timeRemaining = m_startTime;
    }


    public void SetStartTime(long newStartTime)
    {
        m_startTime = newStartTime;
        ClampStartTime();
        m_timeRemaining = m_startTime;
    }


    public void DecrementStartTime()
    {
        m_startTime -= k_IncrementInterval;
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


    public void Tick()
    {
        // This flag allows the timer to be reset when the timer state is ticking or paused!
        m_canResetTimer = m_state == ETimerState.Ticking || m_state == ETimerState.Paused;

        // Just continue ticking if we are allowed to tick!
        if (m_state == ETimerState.Ticking)
        {
            m_timeRemaining -= SecondsToMilliseconds(Time.fixedUnscaledDeltaTime);
            if (m_timeRemaining <= 0)
            {
                m_timeRemaining = 0;
                StopTimer();
            }
        }
    }
}
