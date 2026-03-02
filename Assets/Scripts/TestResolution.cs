using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestResolution : MonoBehaviour
{
    private bool isfullscreen;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
    
    
        
        if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            isfullscreen = !isfullscreen;
            Screen.SetResolution(Screen.width, Screen.height, isfullscreen);
        }
       
        if(Keyboard.current.digit1Key.wasPressedThisFrame) 
        {
            Screen.SetResolution(1920, 1080, isfullscreen);
        }

        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Screen.SetResolution(1280,720, isfullscreen);
        }

        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Screen.SetResolution(1680, 1050, isfullscreen);
        }
        if(Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            Screen.SetResolution(1920, 1200, isfullscreen);
        }
    }
}
