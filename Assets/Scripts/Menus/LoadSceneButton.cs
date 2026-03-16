using System;
using Menus;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour, IMenuButton
{
 [SerializeField] private string sceneName;
 public static Action<string> OnLoadSceneButtonPressed;
 public void OnClickMenuButton()
 {
  if (sceneName == "")
  {
   Debug.Log("Scene name not assigned on button: " + gameObject.name);
   return;
  }
  OnLoadSceneButtonPressed?.Invoke(sceneName);
 }
}
