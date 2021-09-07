using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public event Action onPauseRequested;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseRequested?.Invoke();
        }
    }
}