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
    
    [SerializeField] private Canvas PauseScreen;
    [SerializeField] private SpriteRenderer PauseBackground;

    [SerializeField] private GameObject PauseMenuPrefab;
    private GameObject PauseMenuUI;

    [SerializeField] private Animator fadeAnim;
    
    [SerializeField] private GameObject WinUI;


    private static UIManager _instance; 
    public static UIManager Instance {get{return _instance;}}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            _instance = this;
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
        PauseManager.OnGamePaused += EnableDisablePauseUI;
    }

    private void OnDisable()
    {
        LevelExit.OnLevelExit -= InstantiateWinUI;
        PauseManager.OnGamePaused -= EnableDisablePauseUI;
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
            ZeroMaskTime();
        }
    }

    public void ZeroMaskTime()
    {
        maskDurabilityText.text = "";
        maskIcon.sprite = masks[3].maskSprite;
        player.currentMask = null;
    }

    public void PlayFadeTransition()
    {
        fadeAnim.Play("FadeInOut");
    }

    public void StopFadeTransition()
    {
        fadeAnim.Play("Transparent");
    }

    public void EnableDisablePauseUI(bool isPaused)
    {
        if (PauseMenuUI == null)
        {
            PauseMenuUI = Instantiate(PauseMenuPrefab);
        }
    
        if (isPaused)
        {
           // PauseScreen.enabled = true; 
         //   PauseBackground.enabled = true;
            PauseMenuUI.SetActive(true);
            SetCursorState(true, CursorLockMode.None);
        }
        else
        {
          //  PauseScreen.enabled = false; 
         //   PauseBackground.enabled = false;
             PauseMenuUI.SetActive(false);
            SetCursorState(false, CursorLockMode.Locked);
        }
    }

    public static void SetCursorState(bool visible, CursorLockMode cursorLockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = cursorLockMode;
    }

    private void InstantiateWinUI()
    {
        Instantiate(WinUI);
        SetCursorState(true, CursorLockMode.None);
    }
}
