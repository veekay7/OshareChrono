using DG.Tweening;
using Lean.Gui;
using System;
using UnityEngine;

public class ChronographController : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_countdownTimerGroup = null;
    [SerializeField] private CanvasGroup m_clockGroup = null;
    [SerializeField] private CanvasGroup m_stopwatchGroup = null;
    [SerializeField] private LeanSwitch m_tabBarNavSwitch = null;

    private CanvasGroup m_curShownGroup;


    private void Start()
    {
        ShowGroupImmediate(m_countdownTimerGroup, false);
        ShowGroupImmediate(m_clockGroup, false);
        ShowGroupImmediate(m_stopwatchGroup, false);

        int initialState = m_tabBarNavSwitch.State;
        switch (initialState)
        {
            case 0:
                ShowGroupImmediate(m_countdownTimerGroup, true);
                break;

            case 1:
                ShowGroupImmediate(m_clockGroup, true);
                break;

            case 2:
                ShowGroupImmediate(m_stopwatchGroup, true);
                break;
        }
    }


    private void OnEnable()
    {
        m_tabBarNavSwitch.OnChangedState.AddListener(Cb_OnTabNavBarStateChanged);
    }


    private void OnDisable()
    {
        m_tabBarNavSwitch.OnChangedState.RemoveListener(Cb_OnTabNavBarStateChanged);
    }


    private void ShowGroupImmediate(CanvasGroup group, bool show)
    {
        group.alpha = show ? 1.0f : 0.0f;
        group.interactable = group.blocksRaycasts = show;
        m_curShownGroup = show ? group : null;
    }


    private void ShowGroupLerp(CanvasGroup group, bool show, Action onComplete = null)
    {
        // If we are not going to show this group, then we must disable interactivity and unblock raycast.
        if (!show)
            group.interactable = group.blocksRaycasts = false;

        DOTween.To(() => group.alpha, (a) => group.alpha = a, show ? 1.0f : 0.0f, 0.1f).OnComplete(() =>
        {
            if (show)
            {
                group.interactable = group.blocksRaycasts = true;
                m_curShownGroup = group;
            }

            if (onComplete != null)
                onComplete();
        });
    }


    private void Cb_OnTabNavBarStateChanged(int newState)
    {
        switch (newState)
        {
            case 0:
                ShowGroupLerp(m_curShownGroup, false, () => ShowGroupLerp(m_countdownTimerGroup, true));
                break;

            case 1:
                ShowGroupLerp(m_curShownGroup, false, () => ShowGroupLerp(m_clockGroup, true));
                break;

            case 2:
                ShowGroupLerp(m_curShownGroup, false, () => ShowGroupLerp(m_stopwatchGroup, true));
                break;
        }
    }
}
