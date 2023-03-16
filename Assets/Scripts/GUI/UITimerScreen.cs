using DG.Tweening;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimerScreen : MonoBehaviour
{
    [SerializeField] private ChronoInterface m_chronoInterface = null;

    [Header("GUI Components")]
    [SerializeField] private Image m_timerRadialFillImg = null;
    [SerializeField] private TMP_Text m_timeRemainingTxt = null;
    [SerializeField] private CanvasGroup m_timerGroup = null;
    [SerializeField] private CanvasGroup m_timerAlarmGroup = null;
    [SerializeField] private LeanShake m_timerDismissBtn = null;

    [Header("Buttons")]
    [SerializeField] private Button m_startBtn = null;
    [SerializeField] private Button m_pauseBtn = null;
    [SerializeField] private Button m_resetBtn = null;
    [SerializeField] private Button m_refreshBtn = null;
    [SerializeField] private Button m_increaseTimeBtn = null;
    [SerializeField] private Button m_decreaseTimeBtn = null;

    private CountdownTimer m_countdownTimer;
    private AudioSource m_audioSrc;


    private void Awake()
    {
        m_audioSrc = GetComponent<AudioSource>();
    }


    private void OnDestroy()
    {
        m_countdownTimer.OnTimerFinishedEvent.RemoveListener(Cb_PlayAlarmOnTimerFinished);
    }


    private void Start()
    {
        if (m_chronoInterface != null)
        {
            m_countdownTimer = m_chronoInterface.Timer as CountdownTimer;
            m_countdownTimer.OnTimerFinishedEvent.AddListener(Cb_PlayAlarmOnTimerFinished);
        }  

        // Initialise the initial state of the timer screen!
        // The increase/decrease time button can be seen and interacted with!
        CanvasGroup increaseBtnGroup = m_increaseTimeBtn.GetComponent<CanvasGroup>();
        CanvasGroup decreaseBtnGroup = m_decreaseTimeBtn.GetComponent<CanvasGroup>();
        increaseBtnGroup.alpha = decreaseBtnGroup.alpha = 1.0f;
        increaseBtnGroup.interactable = increaseBtnGroup.blocksRaycasts = decreaseBtnGroup.interactable = decreaseBtnGroup.blocksRaycasts = true;

        // The reset button is deactivated by default!
        m_resetBtn.GetComponent<CanvasGroup>().interactable = false;

        // The refresh button is activated by default!
        m_refreshBtn.GetComponent<CanvasGroup>().interactable = true;

        // Start button is shown by default!
        m_startBtn.gameObject.SetActive(true);

        // Pause button is hidden by default!
        m_pauseBtn.gameObject.SetActive(false);

        // The timer screen is shown by default!
        m_timerGroup.alpha = 1.0f;
        m_timerGroup.interactable = m_timerGroup.blocksRaycasts = true;

        // The timer alarm screen is hidden by default!
        m_timerAlarmGroup.alpha = 0.0f;
        m_timerAlarmGroup.interactable = m_timerAlarmGroup.blocksRaycasts = false;
    }


    private void LateUpdate()
    {
        if (m_timerRadialFillImg != null)
        {
            float fillAmount = m_countdownTimer.TimeRemainingMilliseconds / (float)m_countdownTimer.StartTimeMilliseconds;
            m_timerRadialFillImg.fillAmount = fillAmount;
        }

        if (m_timeRemainingTxt != null)
        {
            m_timeRemainingTxt.text = string.Format("{0:00}:{1:00}",
                m_countdownTimer.TimeRemaining.Minutes, m_countdownTimer.TimeRemaining.Seconds);
        }
    }


    public void Callback_BtnOnClick(Button target)
    {
        if (target == m_startBtn)
        {
            // When start button is pressed, the following will happen:
            // 1. Hide Increase/Decrease time button.
            CanvasGroup increaseTimeBtnGroup = m_increaseTimeBtn.GetComponent<CanvasGroup>();
            increaseTimeBtnGroup.interactable = false;
            DOTween.To(() => increaseTimeBtnGroup.alpha, (a) => increaseTimeBtnGroup.alpha = a, 0.0f, 0.5f);

            CanvasGroup decreaseTimeBtnGroup = m_decreaseTimeBtn.GetComponent<CanvasGroup>();
            decreaseTimeBtnGroup.interactable = false;
            DOTween.To(() => decreaseTimeBtnGroup.alpha, (a) => decreaseTimeBtnGroup.alpha = a, 0.0f, 0.5f);

            // 2. Hide Start button.
            m_startBtn.gameObject.SetActive(false);

            // 3. Show Pause button.
            m_pauseBtn.gameObject.SetActive(true);

            // 4. Activate Reset button.
            m_resetBtn.GetComponent<CanvasGroup>().interactable = true;

            // 5. Deactivate Refresh button.
            m_refreshBtn.GetComponent<CanvasGroup>().interactable = false;

            m_countdownTimer.StartTimer();
        }
        else if (target == m_pauseBtn)
        {
            m_countdownTimer.PauseTimer();

            // When we are paused, the following will happen:
            // 1. Hide Pause button.
            m_pauseBtn.gameObject.SetActive(false);

            // 2. Show the Start button, so we can resume the timer.
            m_startBtn.gameObject.SetActive(true);
        }
        else if (target == m_resetBtn)
        {
            m_countdownTimer.StopTimer();
            Cb_ResetTimer();
        }
        else if (target == m_refreshBtn)
        {
            m_countdownTimer.ResetDefaults();
        }
        else if (target == m_increaseTimeBtn)
        {
            m_countdownTimer.IncrementStartTime();
        }
        else if (target == m_decreaseTimeBtn)
        {
            m_countdownTimer.DecrementStartTime();
        }
    }


    private void Cb_ResetTimer()
    {
        // When the reset button is pressed, the following will happen:
        // 1. Start button is shown!
        m_startBtn.gameObject.SetActive(true);

        // 2. Pause button is hidden!
        m_pauseBtn.gameObject.SetActive(false);

        // 3. Reset button is not interactable!
        m_resetBtn.GetComponent<CanvasGroup>().interactable = false;

        // 4. Refresh button is interactable!
        m_refreshBtn.GetComponent<CanvasGroup>().interactable = true;

        // 5. Show both increase/decrease time buttons!
        CanvasGroup increaseTimeBtnGroup = m_increaseTimeBtn.GetComponent<CanvasGroup>();
        DOTween.To(() => increaseTimeBtnGroup.alpha, (a) => increaseTimeBtnGroup.alpha = a, 1.0f, 0.5f).OnComplete(() =>
        {
            increaseTimeBtnGroup.interactable = true;
        });

        CanvasGroup decreaseTimeBtnGroup = m_decreaseTimeBtn.GetComponent<CanvasGroup>();
        DOTween.To(() => decreaseTimeBtnGroup.alpha, (a) => decreaseTimeBtnGroup.alpha = a, 1.0f, 0.5f).OnComplete(() =>
        {
            decreaseTimeBtnGroup.interactable = true;
        });
    }


    private void Cb_PlayAlarmOnTimerFinished()
    {
        Cb_ResetTimer();

        if (m_audioSrc != null)
            m_audioSrc.Play();

        if (m_timerGroup != null)
            m_timerGroup.interactable = m_timerGroup.blocksRaycasts = false;

        if (m_timerAlarmGroup != null)
        {
            DOTween.To(() => m_timerAlarmGroup.alpha, (a) => m_timerAlarmGroup.alpha = a, 1.0f, 0.15f).OnComplete(() =>
            {
                m_timerDismissBtn.Shake(10);
                m_timerAlarmGroup.interactable = m_timerAlarmGroup.blocksRaycasts = true;
            });
        }
    }


    public void Cb_DismissAlarm()
    {
        if (m_audioSrc != null)
        {
            m_audioSrc.Stop();
        }

        if (m_timerAlarmGroup != null)
        {
            DOTween.To(() => m_timerAlarmGroup.alpha, (a) => m_timerAlarmGroup.alpha = a, 0.0f, 0.15f).OnComplete(() =>
            {
                m_timerAlarmGroup.interactable = m_timerAlarmGroup.blocksRaycasts = false;

                if (m_timerGroup != null)
                    m_timerGroup.interactable = m_timerGroup.blocksRaycasts = true;
            });
        }
    }
}
