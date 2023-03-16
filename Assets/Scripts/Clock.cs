using System;
using System.Collections.ObjectModel;
using UnityEngine;

public class Clock : MonoBehaviour, IClock
{
    [SerializeField] private ChronoInterface m_chronoInterface = null;

    private ReadOnlyCollection<TimeZoneInfo> m_timezones;
    private DateTime m_curDateTime;
    private TimeZoneInfo m_curTimeZone;

    public DateTime CurrentDateTime { get => m_curDateTime; }

    public TimeZoneInfo CurrentTimeZone { get => m_curTimeZone; }


    private void Awake()
    {
        m_chronoInterface.Clock = this;
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

