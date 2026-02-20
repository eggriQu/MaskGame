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
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private PlayerController player;
    public List<Mask> masks;
    [SerializeField] private List<Image> maskIcons;
    [SerializeField] private List<TextMeshProUGUI> maskTimes;

    [SerializeField] private GameObject WinUI;

    private InputAction next;
    private InputAction previous;


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
        next = InputSystem.actions.FindAction("Next");
        previous = InputSystem.actions.FindAction("Previous");

        next.Enable();
        next.performed += NextMask;
        previous.Enable();
        previous.performed += PreviousMask;

        LevelExit.OnLevelExit += InstantiateWinUI;
    }

    private void OnDisable()
    {
        LevelExit.OnLevelExit -= InstantiateWinUI;
    }

    public void NextMask(InputAction.CallbackContext context)
    {
        swipeController.Next();
    }

    public void PreviousMask(InputAction.CallbackContext context)
    {
        swipeController.Previous();
    }

    public void SetJumpMaskPage(Mask mask)
    {
        swipeController.JumpPage();
        maskIcons[0].sprite = mask.maskSprite;
    }

    public void SetSprintMaskPage(Mask mask)
    {
        swipeController.SprintPage();
        maskIcons[1].sprite = mask.maskSprite;
    }

    public void SetDashMaskPage(Mask mask)
    {
        swipeController.DashPage();
        maskIcons[2].sprite = mask.maskSprite;
    }

    public IEnumerator SetMaskTime(Mask mask)
    {
        float time = mask.breakTime;
        while (time > 0)
        {
            if (mask.maskType == 0)
            {
                maskTimes[0].text = "" + time;
            }
            else if (mask.maskType == 1)
            {
                maskTimes[1].text = "" + time;
            }
            time = time - 1;
            yield return new WaitForSeconds(1);
        }
    }

    public void SetDashMaskUses(float uses)
    {
        if (uses > 0)
        {
            maskTimes[2].text = "" + uses;
        }
        else
        {
            maskTimes[2].text = "";
            SetDashMaskPage(masks[3]);
        }
    }

    public void ZeroMaskTime(Mask mask)
    {
        if (mask.maskType == 0)
        {
            maskTimes[0].text = "";
        }
        else if (mask.maskType == 1)
        {
            maskTimes[1].text = "";
        }
        else if (mask.maskType == 2)
        {
            maskTimes[2].text = "";
        }
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
