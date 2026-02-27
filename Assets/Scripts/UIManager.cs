using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public List<Mask> masks;
    [SerializeField] private Image maskIcon;
    [SerializeField] private TextMeshProUGUI maskDurabilityText;

    [SerializeField] private GameObject WinUI;


    private static UIManager _instance; 
    public static UIManager Instance {get{return _instance;}}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        LevelExit.OnLevelExit += InstantiateWinUI;
    }

    private void OnDisable()
    {
        LevelExit.OnLevelExit -= InstantiateWinUI;
    }

    public void SetMaskIcon(Mask mask)
    {
        maskIcon.sprite = mask.maskSprite;
    }

    public IEnumerator SetMaskTime(Mask mask)
    {
        float time = mask.breakTime;
        while (time > 0)
        {
            maskDurabilityText.text = "" + time;
            time = time - 1;
            yield return new WaitForSeconds(1);
        }
    }

    public void SetMaskUses(float uses)
    {
        if (uses > 0)
        {
            maskDurabilityText.text = "" + uses;
        }
        else
        {
            maskDurabilityText.text = "";
            maskIcon.sprite = masks[3].maskSprite;
        }
    }

    public void ZeroMaskTime(Mask mask)
    {
        maskDurabilityText.text = "";
    }

    public void InstantiateDeathUI()
    {
        throw new NotImplementedException();
    }

    private void InstantiateWinUI()
    {
        Instantiate(WinUI);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
