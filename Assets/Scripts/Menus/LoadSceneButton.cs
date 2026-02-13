using Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour, IMenuButton
{
 [SerializeField] private string sceneName;
 public void OnClickMenuButton()
 {
  if (sceneName == "")
  {
   Debug.Log("Scene name not assigned on button: " + gameObject.name);
   return;
  }
  SceneManager.LoadScene(sceneName);
 }
}
