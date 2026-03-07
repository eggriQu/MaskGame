using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;
    private InputActionMap PlayerActionMap;
    private InputAction PauseAction;

    [SerializeField] private Canvas PauseScreen;
    [SerializeField] private SpriteRenderer PauseBackground;

    [SerializeField] private Button ReturnButton;

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

    public void StartPause()
    {
        PauseScreen.enabled = true;
        PauseBackground.enabled = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void EndPause()
    {
        PauseScreen.enabled = false;
        PauseBackground.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
