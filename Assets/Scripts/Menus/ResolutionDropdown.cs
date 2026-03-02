using System;
using System.Collections.Generic;
using Menus;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : BaseDropdown
{
    
    [SerializeField] private List<Vector2Int> resolutions;
    
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.options.Clear();
        
        for (int i = 0; i < resolutions.Count; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(resolutions[i].x + "x" + resolutions[i].y));
        }
        
        

        if (PlayerPrefs.HasKey("ResolutionX"))
        {
            int loadedX = PlayerPrefs.GetInt("ResolutionX");
            int loadedY = PlayerPrefs.GetInt("ResolutionY");
            int savedIndex = 0;

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].x == loadedX && resolutions[i].y == loadedY)
                {
                    savedIndex = i;
                    break;
                }
            }
            dropdown.SetValueWithoutNotify(savedIndex);
        }
        
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener(delegate { OnDropdownChanged(dropdown);});
    }


    public override void OnDropdownChanged(TMP_Dropdown dropdown)
    {
        Screen.SetResolution(resolutions[dropdown.value].x, resolutions[dropdown.value].y, Screen.fullScreen);
        Debug.Log(resolutions[dropdown.value].x + "x" + resolutions[dropdown.value].y);
        PlayerPrefs.SetInt("ResolutionX", resolutions[dropdown.value].x);
        PlayerPrefs.SetInt("ResolutionY", resolutions[dropdown.value].y);
    }
    
}
