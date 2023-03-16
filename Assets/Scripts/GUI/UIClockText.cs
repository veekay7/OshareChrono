using TMPro;
using UnityEngine;

public class UIClockText : MonoBehaviour
{
    [SerializeField]
    private ChronoInterface m_clockVariables = null;
    [SerializeField]
    private bool m_timeFormat24Hour = false;
    [SerializeField]
    private bool m_showSeconds = false;
    [SerializeField]
    private TMP_Text m_textComponent = null;

    public bool TimeIn24HourFormat 
    { 
        get => m_timeFormat24Hour; 
        set => m_timeFormat24Hour = value; 
    }


    private void Awake()
    {
        if (m_textComponent == null)
            m_textComponent = GetComponent<TMP_Text>();
    }


    private void LateUpdate()
    {
        if (m_textComponent != null && m_clockVariables != null && m_clockVariables.Clock != null)
        {
            var clock = m_clockVariables.Clock;
            if (m_timeFormat24Hour)
            {
                string format = m_showSeconds ? "HH:mm:ss" : "HH:mm";
                m_textComponent.text = clock.CurrentDateTime.ToString(format);
            }
            else 
            {
                string format = m_showSeconds ? "hh:mm:ss tt" : "hh:mm tt";
                m_textComponent.text = clock.CurrentDateTime.ToString(format);
            } 
        }
    }
}
