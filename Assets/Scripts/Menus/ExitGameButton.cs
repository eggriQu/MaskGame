using System;
using System.Collections;
using Menus;
using UnityEngine;

public class ExitGameButton : MonoBehaviour, IMenuButton
{
 public static Action OnExitGameButtonPressed;

 public void OnClickMenuButton()
 {
  OnExitGameButtonPressed?.Invoke();
 }

 
}
