using Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneButton : MonoBehaviour, IMenuButton
{
    public void OnClickMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
