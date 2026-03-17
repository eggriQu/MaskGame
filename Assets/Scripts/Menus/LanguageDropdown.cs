using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageDropdown : BaseDropdown
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LanguageSelect(int localeIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
    }

    public override void OnDropdownChanged(TMP_Dropdown dropdown)
    {
        if (dropdown.value == 0)
        {
            LanguageSelect(0);
        }
        else if (dropdown.value == 1)
        {
            LanguageSelect(1);
        }
    }
}
