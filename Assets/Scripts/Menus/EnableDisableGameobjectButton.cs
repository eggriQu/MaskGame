using Menus;
using UnityEngine;

public class EnableDisableGameobjectButton : MonoBehaviour, IMenuButton
{
  [SerializeField] private GameObject gameobjectToEnable;
  [SerializeField] private GameObject gameobjectToDisable;

  public void OnClickMenuButton()
  {
    gameobjectToDisable.SetActive(false);
    gameobjectToEnable.SetActive(true);
  }
}
