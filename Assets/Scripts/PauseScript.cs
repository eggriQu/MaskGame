using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;
    private InputActionMap PlayerActionMap;
    private InputAction PauseAction;

    [SerializeField] private Canvas PauseScreen;
    [SerializeField] private SpriteRenderer PauseBackground;

    public bool IsPaused;

    private void Awake()
    {
        PlayerActionMap = InputActions.FindActionMap("Player");
        PauseAction = PlayerActionMap.FindAction("Pause");
    }

    private void Start()
    {
        EndPause();
    }

    private void Update()
    {
        if (PauseAction.WasPressedThisFrame())
        {
            if (IsPaused)
            {
                EndPause();
            }
            else
            {
                StartPause();
            }
        }
    }

    void StartPause()
    {
        PauseScreen.enabled = true;
        PauseBackground.enabled = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
        Time.timeScale = 0f;
    }

    void EndPause()
    {
        PauseScreen.enabled = false;
        PauseBackground.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        IsPaused = false;
        Time.timeScale = 1f;
    }
}
