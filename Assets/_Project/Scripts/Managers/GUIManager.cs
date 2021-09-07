using UnityEngine;
using System;

public class GUIManager : MonoBehaviour
{
    public event Action onDefaultStageRequested;
    public event Action onBuildStageRequested;
    public event Action onRetryRequested;
    public event Action onIntroRequested;
    public event Action<int> onLanguageRequested;

    public Display[] displays;

    private Display _activeDisplay;

    public void Initiate()
    {
        Display.onActionRequested += OnActionRequested;  

        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initiate();
        }
    }

    public void Initialize()
    {
        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initialize();
        }
    }

    private void OnDestroy()
    {
        Display.onActionRequested -= OnActionRequested;
    }

    public void ShowDisplay(Displays p_display)
    {
        if (_activeDisplay == null || (_activeDisplay != null && _activeDisplay.ID != p_display))
        {
            if (_activeDisplay != null)
            {
                _activeDisplay.Show(false);
            }

            _activeDisplay = displays[(byte)p_display];
            _activeDisplay.Show(true);
        }
    }

    public void HideCurrentDisplay()
    {
        _activeDisplay.Show(false);
        _activeDisplay = null;
    }

    public void UpdateDisplay(Displays p_id, int p_operation, int p_value)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_value);
    }

    public void UpdateDisplay(Displays p_id, string p_value)
    {
        displays[(int)p_id].UpdateDisplay(p_value);
    }

    public virtual void UpdateDisplay(Displays p_id, int p_operation)
    {
        displays[(int)p_id].UpdateDisplay(p_operation);
    }

    private void OnActionRequested(Displays p_id, int p_action)
    {
        switch(p_id)
        {
            case Displays.LANGUAGE:
                switch (p_action)
                {
                    case 0:
                        onLanguageRequested?.Invoke((int)Languages.PORTUGUES);
                        break;
                    case 1:
                        onLanguageRequested?.Invoke((int)Languages.ENGLISH);
                        break;
                }
                break;
            case Displays.INTRO:
                switch(p_action)
                {
                    case 0:
                        onDefaultStageRequested?.Invoke();
                        break;
                    case 1:
                        onBuildStageRequested?.Invoke();
                        break;
                }
                break;
            case Displays.GAME_OVER:
                switch (p_action)
                {
                    case 0:
                        onRetryRequested?.Invoke();
                        break;
                    case 1:
                        onIntroRequested?.Invoke();
                        break;
                }
                break;
        }
    }
}