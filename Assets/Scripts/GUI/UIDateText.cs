using TMPro;
using UnityEngine;

public class UIDateText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_textComponent;
    [SerializeField]
    private ChronoInterface m_clockVariables = null;
    [SerializeField]
    private bool m_numberFormat = false;


    private void Awake()
    {
        if (m_textComponent == null)
            m_textComponent = GetComponent<TMP_Text>();
    }


    private void LateUpdate()
    {
        if (m_textComponent != null && m_clockVariables != null )
        {
            var clock = m_clockVariables.Clock;
            if (clock != null)
            {
                if (m_numberFormat)
                    m_textComponent.text = m_clockVariables.Clock.CurrentDateTime.ToString("dddd, dd/MM/yyyy");
                else
                    m_textComponent.text = m_clockVariables.Clock.CurrentDateTime.ToString("dddd, dd MMMM yyyy");
            }
        }
    }
}
