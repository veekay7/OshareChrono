using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using UnityEngine;

public class ClockTests
{
    [Test]
    public void ShowOSTimeZoneList()
    {
        ReadOnlyCollection<TimeZoneInfo> timezones = TimeZoneInfo.GetSystemTimeZones();
        for (int i = 0; i < timezones.Count; i++)
        {
            var zone = timezones[i];
            Debug.LogFormat($"{i}. {zone.Id} => {zone.DisplayName}");
        }
    }


    [Test]
    public void GetLocalTimeFromUTCTimeAndLocalTimeZone()
    {
        // 1. Get UTC time from the OS.
        // 2. Get timezone info from the registry of the machine on which it is executing.
        // 3. Use that timezone information to resolve UTC to local time (once again, local to the executing machine)
        ReadOnlyCollection<TimeZoneInfo> timezones = TimeZoneInfo.GetSystemTimeZones();
        TimeZoneInfo selectedTimeZone = timezones[113];
        DateTime utcTime = DateTime.UtcNow;
        DateTime newLocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, selectedTimeZone.Id);
        Debug.LogFormat("\nNew Local Time: {0} {1}", newLocalTime, selectedTimeZone.StandardName);
    }
}
