using System;
using System.Collections.ObjectModel;

public interface IClock
{
    void SetTimeZone(TimeZoneInfo newTimeZone);

    ReadOnlyCollection<TimeZoneInfo> GetTimeZones();

    public DateTime CurrentDateTime { get; }

    public TimeZoneInfo CurrentTimeZone { get; }
}
