using System;
using TMPro;
using UnityEngine;

public class UILapTimeItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_lapNumTxt = null;
    [SerializeField] private TMP_Text m_lapTimeTxt = null;

    private TimeSpan m_lapTime;
    
    public RectTransform rectTransform { get => (RectTransform)transform; }


    public void SetLapNum(int lapNum)
    {
        if (m_lapNumTxt == null)
            return;

        m_lapNumTxt.text = Convert.ToString(lapNum);
    }


    public void SetLapTime(TimeSpan lapTime)
    {
        if (m_lapTimeTxt == null)
            return;

        m_lapTime = lapTime;
        m_lapTimeTxt.text = string.Format("{0:00}:{1:00}.{2:000}", lapTime.Minutes, lapTime.Seconds, lapTime.Milliseconds);
    }
}
