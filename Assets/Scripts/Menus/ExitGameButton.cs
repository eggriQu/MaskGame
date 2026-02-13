using Menus;
using UnityEngine;

public class ExitGameButton : MonoBehaviour, IMenuButton
{
 public void OnClickMenuButton()
 {
  Application.Quit();
  Debug.Log("Game closed");
 }
}
