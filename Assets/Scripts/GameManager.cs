using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private PlayerController player;
    public List<Mask> masks;
    [SerializeField] private List<Image> maskIcons;

    private InputAction next;
    private InputAction previous;

    private void OnEnable()
    {
        next = InputSystem.actions.FindAction("Next");
        previous = InputSystem.actions.FindAction("Previous");

        next.Enable();
        next.performed += NextMask;
        previous.Enable();
        previous.performed += PreviousMask;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
