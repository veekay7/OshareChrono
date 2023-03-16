using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIClockScreen : MonoBehaviour
{
    [Header("Clock reference")]
    [SerializeField] private ChronoInterface m_clockVariables = null;

    [Header("GUI Components")]
    [SerializeField] private Image m_hourSliderBgImg = null;
    [SerializeField] private Image m_hourSliderFillImg = null;
    [SerializeField] private Image m_minuteSliderBgImg = null;
    [SerializeField] private Image m_minuteSliderFillImg = null;
    [SerializeField] private Image m_secondsSliderBgImg = null;
    [SerializeField] private Image m_secondsSliderFillImg = null;

    [Header("Hour hand colours")]
    [SerializeField] private Color m_daytimeBgColour = Color.grey;
    [SerializeField] private Color m_nighttimeBgColour = Color.grey;
    [SerializeField] private Color m_daytimeColour = Color.red;
    [SerializeField] private Color m_nighttimeColour = Color.blue;

    private bool m_initHourHandColours;
    private bool m_lerpHourHandColour;


    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (m_clockVariables != null && m_clockVariables.Clock != null)
            {
                var clock = m_clockVariables.Clock;
                var datum = clock.CurrentDateTime;
                if (datum.Hour >= 6 && datum.Hour < 18)
                {
                    m_hourSliderBgImg.color = m_daytimeBgColour;
                }

                if (datum.Hour < 6 && datum.Hour <= 18)
                {
                    m_hourSliderBgImg.color = m_nighttimeBgColour;
                }
            }
        }
    }


    private void Start()
    {
        m_hourSliderFillImg.fillAmount = 0.0f;
        m_minuteSliderFillImg.fillAmount = 0.0f;
        m_secondsSliderFillImg.fillAmount = 0.0f;

        m_initHourHandColours = true;
        m_lerpHourHandColour = false;
    }

    private void LateUpdate()
    {
        if (m_clockVariables != null && m_clockVariables.Clock != null)
        {
            var clock = m_clockVariables.Clock;

            // Continually get the local date and time!
            var datum = clock.CurrentDateTime;

            // Update the seconds hand.
            m_secondsSliderFillImg.fillAmount = datum.Second / 60.0f;

            // Update the minutes hand.
            m_minuteSliderFillImg.fillAmount = datum.Minute / 60.0f;

            // Update the hour hand.
            m_hourSliderFillImg.fillAmount = datum.Hour / 24.0f;
            
            if (m_initHourHandColours)
            {
                var timeOfDay = datum.TimeOfDay;
                if (timeOfDay >= TimeSpan.Parse("06:30"))
                {
                    m_hourSliderBgImg.color = m_daytimeBgColour;
                    m_hourSliderFillImg.color = m_daytimeColour;
                }
                
                if (timeOfDay >= TimeSpan.Parse("18:30"))
                {
                    m_hourSliderBgImg.color = m_nighttimeBgColour;
                    m_hourSliderFillImg.color = m_nighttimeColour;
                }

                m_initHourHandColours = false;
            }

            // Colour swap on the minutes hand when we reached 59 minutes, 59 seconds, and 999 milliseconds.
            if (datum.Minute > 59 && datum.Second >= 59 && datum.Millisecond >= 999)
            {
                ColourSwap(m_minuteSliderBgImg, m_minuteSliderFillImg);
            }

            // Colour swap on the seconds hand when re reached 59 seconds and 999 milliseconds.
            if (datum.Second == 59 && datum.Millisecond == 999)
            {
                ColourSwap(m_secondsSliderBgImg, m_secondsSliderFillImg);
            }

            // Lerp the colour of the hour hand depending on time of day.
            // Following the conventions of the Rolex GMT-Master II,
            // Day time = 0630 hrs to 1829 hrs.
            // Night time = 1830 hrs to 0629 hrs.
            if (datum.Minute == 30 && (datum.Hour == 6 || datum.Hour == 18))
            {
                // If we aren't lerping the hour hand colour, we can lerp it!
                if (!m_lerpHourHandColour)
                {
                    m_lerpHourHandColour = true;

                    int lerpMode = 0;
                    if (datum.Hour == 6)
                        lerpMode = 1;
                    else if (datum.Hour == 18)
                        lerpMode = -1;

                    StartCoroutine(Co_LerpHourHandColour(lerpMode));
                }
            }
        }
    }


    private void ColourSwap(Image bgImg, Image fillImg)
    {
        // Switch the colour with the background and reset the slider fill!
        // It also works vice versa!
        Color prevFillColour = fillImg.color;
        fillImg.color = bgImg.color;
        bgImg.color = prevFillColour;
        fillImg.fillAmount = 0.0f;
    }

    
    /// <summary>
    /// Coroutine to lerp the hour hand colour.
    /// </summary>
    /// <param name="mode">
    /// 1 - Daytime
    /// -1 - Night time
    /// </param>
    /// <returns></returns>
    private IEnumerator Co_LerpHourHandColour(int mode)
    {
        Color startBgColour, targetBgColour;
        Color startFillColour, targetFillColour;

        if (mode == 1)
        {
            startBgColour = m_nighttimeBgColour;
            targetBgColour = m_daytimeBgColour;
            startFillColour = m_nighttimeColour;
            targetFillColour = m_daytimeColour;
        }
        else
        {
            startBgColour = m_daytimeBgColour;
            targetBgColour = m_nighttimeBgColour;
            startFillColour = m_daytimeColour;
            targetFillColour = m_nighttimeColour;
        }

        m_hourSliderBgImg.color = startBgColour;
        m_hourSliderFillImg.color = startFillColour;

        float curLerpTime = 0.0f;
        float maxLerpTime = 3.0f;
        while (true)
        {
            curLerpTime += Time.deltaTime;
            float t = curLerpTime / maxLerpTime;

            m_hourSliderBgImg.color = Color.Lerp(startBgColour, targetBgColour, t);
            m_hourSliderFillImg.color = Color.Lerp(startFillColour, targetFillColour, t);
            
            if (curLerpTime >= maxLerpTime)
            {
                break;
            }

            yield return null;
        }

        // Okay, we are out of the loop, let's wait for 1 minute after to unlock the m_lerpHourHandColour flag.
        yield return new WaitForSecondsRealtime(1.1f);
        m_lerpHourHandColour = false;
    }
}
