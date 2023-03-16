using System;

public interface ITimer
{
    public TimeSpan StartTime { get; }

    public long StartTimeMilliseconds { get; }


    void StartTimer();

    void StopTimer();

    void ResetTimer();

    void PauseTimer();

    void ResetDefaults();
}
