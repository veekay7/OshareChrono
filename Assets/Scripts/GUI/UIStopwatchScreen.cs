using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStopwatchScreen : MonoBehaviour
{
    [SerializeField] private ChronoInterface m_chronoInterface = null;
    [SerializeField] private TMP_Text m_elapsedTimeTxt = null;
    [SerializeField] private TMP_Text m_splitTimeTxt = null;
    [SerializeField] private UILapTimeItem m_lapTimeItemPrefab = null;
    [SerializeField] private RectTransform m_lapTimeListContentXform = null;

    [Header("Buttons")]
    [SerializeField] private Button m_startBtn = null;
    [SerializeField] private Button m_stopBtn = null;
    [SerializeField] private Button m_splitTimeBtn = null;
    [SerializeField] private Button m_resetBtn = null;

    private StopwatchTimer m_stopwatch;
    private List<UILapTimeItem> m_lapTimeItems = new List<UILapTimeItem>();


    private void Start()
    {
        if (m_chronoInterface != null)
            m_stopwatch = m_chronoInterface.Stopwatch as StopwatchTimer;

        // Let's set default state for the buttons.
        m_startBtn.gameObject.SetActive(true);
        m_stopBtn.gameObject.SetActive(false);
        m_splitTimeBtn.interactable = false;
        m_resetBtn.interactable = true;
    }


    private void LateUpdate()
    {
        if (m_stopwatch != null)
        {
            var curElapsedTime = m_stopwatch.CurrentElapsedTime;
            var curSplitTime = m_stopwatch.CurrentSplitTime;

            m_elapsedTimeTxt.text = string.Format("{0:00}:{1:00}.{2:000}",
                curElapsedTime.Minutes, curElapsedTime.Seconds, curElapsedTime.Milliseconds);

            m_splitTimeTxt.text = string.Format("{0:00}:{1:00}.{2:000}",
                curSplitTime.Minutes, curSplitTime.Seconds, curSplitTime.Milliseconds);
        }
    }


    public void StartStopwatch()
    {
        if (m_stopwatch == null)
            return;

        m_stopwatch.StartStopwatch();

        // When the stopwatch has begun ticking, we will need to do the following:
        // 1. Hide Start button.
        m_startBtn.gameObject.SetActive(false);

        // 2. Show Stop button.
        m_stopBtn.gameObject.SetActive(true);

        // 3. Reset button is inactive.
        m_resetBtn.interactable = false;

        // 4. Split time button is active.
        m_splitTimeBtn.interactable = true;
    }


    public void SplitTime()
    {
        if (m_stopwatch == null)
            return;

        var lapTime = m_stopwatch.SplitTime();

        var newLapTimeItem = Instantiate(m_lapTimeItemPrefab);
        newLapTimeItem.SetLapNum(m_lapTimeItems.Count + 1);
        newLapTimeItem.SetLapTime(lapTime);
        newLapTimeItem.rectTransform.SetParent(m_lapTimeListContentXform.transform, false);
        newLapTimeItem.rectTransform.SetAsFirstSibling();
        newLapTimeItem.gameObject.SetActive(true);

        m_lapTimeItems.Add(newLapTimeItem);
    }


    public void StopStopwatch()
    {
        if (m_stopwatch == null)
            return;

        m_stopwatch.StopStopwatch();

        // When the stopwatch has stopped ticking, we will need to do the following:
        // 1. Show Start button.
        m_startBtn.gameObject.SetActive(true);

        // 2. Hide Stop button.
        m_stopBtn.gameObject.SetActive(false);

        // 3. Reset button is active.
        m_resetBtn.interactable = true;

        // 4. Split time button is inactive.
        m_splitTimeBtn.interactable = false;
    }


    public void ResetStopwatch()
    {
        if (m_stopwatch == null)
            return;

        m_stopwatch.ResetStopwatch();

        // Destroy all the lap time items.
        for (int i = 0; i < m_lapTimeItems.Count; i++)
        {
            var item = m_lapTimeItems[i];
            if (item != null)
                Destroy(item.gameObject);
        }
        m_lapTimeItems.Clear();

        // When the stopwatch is reset , we will need to do the following:
        // 3. Reset button is inactive.
        m_resetBtn.interactable = false;

        // 4. Split time button is active.
        m_splitTimeBtn.interactable = false;
    }
}
