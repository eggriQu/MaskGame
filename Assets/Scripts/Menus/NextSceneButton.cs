using System;
using Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour, IMenuButton
{
    public static Action<string> OnLoadSceneButtonPressed;



    public void OnClickMenuButton()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex > SceneManager.sceneCountInBuildSettings-1)
        {
            OnLoadSceneButtonPressed?.Invoke("MainMenu");
        }
        else
        {
            string path = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
            int slash = path.LastIndexOf('/');
            string name = path.Substring(slash + 1);
            int dot = name.LastIndexOf('.');
            string sceneName = name.Substring(0, dot);
            
            
            
            Debug.Log("Loading scene: " + sceneName);
            OnLoadSceneButtonPressed?.Invoke(sceneName);   
        }
    }
}