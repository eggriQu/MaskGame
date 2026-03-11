using Menus;
using UnityEngine;

public class ResumeGameButton : MonoBehaviour, IMenuButton
{
    public void OnClickMenuButton()
    {
        PauseManager.ResumeGame();
    }
}
