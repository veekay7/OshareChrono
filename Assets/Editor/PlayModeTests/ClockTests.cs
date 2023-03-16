using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.TestTools;

public class ClockTests
{
    [UnityTest]
    public IEnumerator TestCurrentTimeZone()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new MonoBehaviourTest<TestClockBehaviour>();
    }
}


public class TestClockBehaviour : MonoBehaviour, IMonoBehaviourTest, IClock
{
    private ReadOnlyCollection<TimeZoneInfo> m_timezones;
    private DateTime m_curDateTime;
    private TimeZoneInfo m_curTimeZone;

    public bool IsTestFinished => TestResults();

    public DateTime CurrentDateTime { get => m_curDateTime; }


    public TimeZoneInfo CurrentTimeZone { get => m_curTimeZone; }


    private bool TestResults()
    {
        return m_curTimeZone.Id == "Tokyo Standard Time";
    }


    private void Awake()
    {
        m_timezones = TimeZoneInfo.GetSystemTimeZones();
        SetTimeZone(TimeZoneInfo.Local);
    }


    private void Start()
    {
        // Find the local timezone of this system and set the time.
        SetTimeZone(TimeZoneInfo.Local);
    }


    private void Update()
    {
        DateTime utcTime = DateTime.UtcNow;
        DateTime newLocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, m_curTimeZone.Id);
        m_curDateTime = newLocalTime;
    }


    public void SetTimeZone(TimeZoneInfo newTimeZone)
    {
        m_curTimeZone = newTimeZone;
    }


    public ReadOnlyCollection<TimeZoneInfo> GetTimeZones()
    {
        return m_timezones;
    }
}
