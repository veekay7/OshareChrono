using TMPro;
using UnityEngine;

public class UITimezoneText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_textComponent = null;
    [SerializeField]
    private ChronoInterface m_chronoInterface = null;


    private void Awake()
    {
        if (m_textComponent == null)
            m_textComponent = GetComponent<TMP_Text>();
    }


    private void LateUpdate()
    {
        if (m_textComponent != null && m_chronoInterface != null && m_chronoInterface.Clock != null)
        {
            var clock = m_chronoInterface.Clock;
            m_textComponent.text = clock.CurrentTimeZone.DisplayName;
        }
    }
}
