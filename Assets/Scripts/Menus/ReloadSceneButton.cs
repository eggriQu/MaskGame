using System;
using UnityEngine;

using Menus;
using UnityEngine;


public class ReloadSceneButton : MonoBehaviour, IMenuButton
{
    public static Action OnReloadSceneButtonPressed;
    public void OnClickMenuButton()
    {
        OnReloadSceneButtonPressed?.Invoke();
    }
}