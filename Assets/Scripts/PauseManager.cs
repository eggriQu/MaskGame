using System;
using UnityEngine;

public static class PauseManager
{
    public static bool isGamePaused { get; private set; }
    public static bool isLevelPaused { get; private set; }
    public static Action<bool> OnGamePaused;
    public static Action<bool> OnLevelPaused;

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
    
    public static void PauseLevel()
    {
        isLevelPaused = true;
        OnLevelPaused?.Invoke(isLevelPaused);
        Debug.Log("PauseLevel");
    }

    public static void ResumeLevel()
    {
        isLevelPaused = false;
        OnLevelPaused?.Invoke(isLevelPaused);
        Debug.Log("ResumeLevel");
    }
    
    
}
