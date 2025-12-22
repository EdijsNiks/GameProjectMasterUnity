using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class HandMenu : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionAsset inputActions;

    [Header("Menu Settings")]
    public float fadeSpeed = 5f;
    public float scaleSpeed = 5f;
    public float targetScale = 1f;

    [Header("UI Buttons")]
    public Button restartButton;
    public Button exitButton;

    private Canvas handMenuCanvas;
    private CanvasGroup canvasGroup;
    private InputAction menuAction;
    private bool isMenuVisible = false;
    private bool isAnimating = false;
    private Vector3 desiredScale;

    private void Awake()
    {
        handMenuCanvas = GetComponent<Canvas>();

        // Add CanvasGroup for fade control
        canvasGroup = handMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = handMenuCanvas.gameObject.AddComponent<CanvasGroup>();

        handMenuCanvas.enabled = false;
        handMenuCanvas.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (inputActions == null)
        {
            Debug.LogError("InputActionAsset not assigned to HandMenuInput!");
            return;
        }

        // Find the Menu action from the Left Hand map
        menuAction = inputActions.FindActionMap("XR LeftHand")?.FindAction("Menu");

        if (menuAction == null)
        {
            Debug.LogError("Couldn't find 'Menu' action in 'XR LeftHand' map!");
            return;
        }

        menuAction.Enable();
        menuAction.performed += ToggleMenu;

        // Assign button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartPressed);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitPressed);
    }

    private void OnDestroy()
    {
        if (menuAction != null)
            menuAction.performed -= ToggleMenu;
    }

    private void Update()
    {
        if (!isAnimating) return;

        // Smooth fade and scale
        handMenuCanvas.transform.localScale = Vector3.Lerp(handMenuCanvas.transform.localScale, desiredScale, Time.unscaledDeltaTime * scaleSpeed);
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, desiredScale == Vector3.zero ? 0f : 1f, Time.unscaledDeltaTime * fadeSpeed);

        if (Vector3.Distance(handMenuCanvas.transform.localScale, desiredScale) < 0.01f)
        {
            isAnimating = false;

            if (desiredScale == Vector3.zero)
            {
                handMenuCanvas.enabled = false;
                Time.timeScale = 1f;
                Debug.Log("Hand Menu Closed — Game Resumed");
            }
        }
    }

    private void ToggleMenu(InputAction.CallbackContext ctx)
    {
        isMenuVisible = !isMenuVisible;
        handMenuCanvas.enabled = true;
        desiredScale = isMenuVisible ? Vector3.one * targetScale : Vector3.zero;
        isAnimating = true;

        if (isMenuVisible)
        {
            Time.timeScale = 0f;
            Debug.Log("Hand Menu Opened — Game Paused");
        }
    }

    // === UI Button Events ===
    public void OnRestartPressed()
    {
        Debug.Log("Restart button pressed — reloading scene.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitPressed()
    {
        Debug.Log("Exit button pressed — quitting game.");
        Time.timeScale = 1f;
        Application.Quit();
    }
}
