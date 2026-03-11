using System;
using UnityEngine;

public static class PauseManager
{
    public static bool isGamePaused { get; private set; }
    public static Action<bool> OnGamePaused;

    public static void PauseGame()
    {
        isGamePaused = true;
        OnGamePaused?.Invoke(isGamePaused);
        Debug.Log("PauseGame");
    }

    public static void ResumeGame()
    {
        isGamePaused = false;
        OnGamePaused?.Invoke(isGamePaused);
        Debug.Log("ResumeGame");
    }
}
