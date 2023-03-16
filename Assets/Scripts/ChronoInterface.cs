using UnityEngine;

[CreateAssetMenu(fileName = "NewChronoInterface", menuName = "Assignment/Chrono Interface Object")]
public class ChronoInterface : ScriptableObject
{
    private IClock m_clock;
    private ITimer m_timer;
    private IStopwatch m_stopwatch;
    

    public IClock Clock
    {
        get => m_clock;
        set => m_clock = value;
    }

    public ITimer Timer
    {
        get => m_timer;
        set => m_timer = value;
    }

    public IStopwatch Stopwatch
    {
        get => m_stopwatch;
        set => m_stopwatch = value;
    }
}
