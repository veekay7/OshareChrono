using System;

public interface IStopwatch
{
    public bool IsRunning { get; }

    public TimeSpan CurrentElapsedTime { get; }

    public TimeSpan CurrentSplitTime { get; }


    void StartStopwatch();

    void StopStopwatch();

    void ResetStopwatch();

    TimeSpan SplitTime();
}
