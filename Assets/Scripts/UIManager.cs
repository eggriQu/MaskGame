using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public List<Mask> masks;
    [SerializeField] private Image maskIcon;
    [SerializeField] private TextMeshProUGUI maskDurabilityText;
    


    [SerializeField] private GameObject PauseMenuPrefab;
    private GameObject PauseMenuUI;


    
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
        PauseManager.OnGamePaused += EnableDisablePauseUI;

        SceneManager.sceneLoaded += FindPlayerUI;
    }

    private void OnDisable()
    {
        LevelExit.OnLevelExit -= InstantiateWinUI;
        PauseManager.OnGamePaused -= EnableDisablePauseUI;
        
        SceneManager.sceneLoaded -= FindPlayerUI;
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



    public void EnableDisablePauseUI(bool isPaused)
    {
        if (isPaused)
        {
            if (!PauseMenuUI)
            { 
              PauseMenuUI = Instantiate(PauseMenuPrefab);
              Debug.Log("Pause Menu UI Instantiated");
            }
            SetCursorState(true, CursorLockMode.None);
        }
        else
        {
            if (PauseMenuUI)
            {
                Destroy(PauseMenuUI);
            }
            SetCursorState(false, CursorLockMode.Locked);
        }
    }

    private void FindPlayerUI(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0){return;}
        maskIcon = GameObject.Find("Current Mask Icon").GetComponent<Image>();
        maskDurabilityText = GameObject.Find("Durability Text").GetComponent<TextMeshProUGUI>();
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
