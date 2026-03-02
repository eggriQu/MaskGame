using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour, IToggle
{
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = Screen.fullScreen;
    }
    
    public void OnToggle()
    {
        Screen.fullScreen = toggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", toggle.isOn ? 1 : 0);
    }
}
