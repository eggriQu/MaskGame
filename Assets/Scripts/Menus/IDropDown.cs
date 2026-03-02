using System;
using TMPro;
using UnityEngine;


public interface IDropDown
{
    public void OnDropdownChanged(TMP_Dropdown dropdown);
}

public abstract class BaseDropdown : MonoBehaviour, IDropDown
{
    protected TMP_Dropdown dropdown;


    public abstract void OnDropdownChanged(TMP_Dropdown dropdown);
}
