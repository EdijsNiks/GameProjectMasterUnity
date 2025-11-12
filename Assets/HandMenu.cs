using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;
using UnityEngine.UI;
using System.Collections;

public class HandMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public GameObject menuUI;                  // The canvas or panel for the menu
    public Grabber leftHandGrabber;            // Assign your LeftHandController (with Grabber)
    public float menuDistance = 0.15f;         // Distance from hand
    public Vector3 menuOffset = new Vector3(0f, 0.1f, 0f); // Offset above the hand

    [Header("Animation Settings")]
    public float appearSpeed = 6f;             // Speed of appear animation
    public float scaleTarget = 1f;             // Final scale of menu
    public float fadeSpeed = 6f;               // Speed of fade (if using CanvasGroup)

    [Header("Input Settings")]
    public InputBridge input;

    private bool menuActive = false;
    private Vector3 targetScale;
    private CanvasGroup canvasGroup;
    private bool isAnimating = false;

    void Start()
    {
        if (input == null)
            input = InputBridge.Instance;

        if (menuUI != null)
        {
            targetScale = Vector3.zero;
            menuUI.transform.localScale = Vector3.zero;

            canvasGroup = menuUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = menuUI.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            menuUI.SetActive(false);
        }
    }

    void Update()
    {
        if (input == null || leftHandGrabber == null || menuUI == null)
            return;

        // X button toggles menu
        if (input.XButtonDown)
        {
            ToggleMenu();
        }

        // Smooth scale + fade animation
        if (isAnimating)
        {
            menuUI.transform.localScale = Vector3.Lerp(menuUI.transform.localScale, targetScale, Time.deltaTime * appearSpeed);
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetScale == Vector3.zero ? 0f : 1f, Time.deltaTime * fadeSpeed);

            if (Vector3.Distance(menuUI.transform.localScale, targetScale) < 0.01f)
            {
                isAnimating = false;
                if (targetScale == Vector3.zero)
                    menuUI.SetActive(false);
            }
        }

        // Update position if menu is open
        if (menuActive)
        {
            UpdateMenuPosition();
        }
    }

    void ToggleMenu()
    {
        menuActive = !menuActive;

        if (menuActive)
        {
            menuUI.SetActive(true);
            targetScale = Vector3.one * scaleTarget;
            Time.timeScale = 0f; // Pause game
            Debug.Log("Hand Menu Opened — Game Paused");
        }
        else
        {
            targetScale = Vector3.zero;
            Time.timeScale = 1f; // Resume game
            Debug.Log("Hand Menu Closed — Game Resumed");
        }

        isAnimating = true;
    }

    void UpdateMenuPosition()
    {
        Transform hand = leftHandGrabber.transform;
        Vector3 forward = hand.forward;
        Vector3 position = hand.position + forward * menuDistance + hand.TransformDirection(menuOffset);

        menuUI.transform.position = position;
        menuUI.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    // Called by Restart button
    public void OnRestartPressed()
    {
        Debug.Log("Restart button pressed — would reload scene.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called by Exit button
    public void OnExitPressed()
    {
        Debug.Log("Exit button pressed — would quit game.");
        Time.timeScale = 1f;
        Application.Quit();
    }
}
