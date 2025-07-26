// InputManager.cs - Handle input across different game states
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Action References")]
    public InputActionReference pauseAction;
    public InputActionReference confirmAction;
    public InputActionReference cancelAction;

    [Header("References")]
    public GameManager gameManager;

    private void Start()
    {
        // Enable input actions
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }

        if (confirmAction != null)
        {
            confirmAction.action.performed += OnConfirmPressed;
            confirmAction.action.Enable();
        }

        if (cancelAction != null)
        {
            cancelAction.action.performed += OnCancelPressed;
            cancelAction.action.Enable();
        }
    }

    private void OnDestroy()
    {
        // Disable input actions
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }

        if (confirmAction != null)
        {
            confirmAction.action.performed -= OnConfirmPressed;
            confirmAction.action.Disable();
        }

        if (cancelAction != null)
        {
            cancelAction.action.performed -= OnCancelPressed;
            cancelAction.action.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (gameManager.currentState == GameManager.GameState.Playing)
        {
            gameManager.PauseGame();
        }
        else if (gameManager.currentState == GameManager.GameState.Paused)
        {
            gameManager.ResumeGame();
        }
    }

    private void OnConfirmPressed(InputAction.CallbackContext context)
    {
        // Handle confirm action based on current state
        switch (gameManager.currentState)
        {
            case GameManager.GameState.MainMenu:
                gameManager.StartGame();
                break;
            case GameManager.GameState.Paused:
                gameManager.ResumeGame();
                break;
            case GameManager.GameState.GameOver:
                gameManager.RestartGame();
                break;
        }
    }

    private void OnCancelPressed(InputAction.CallbackContext context)
    {
        // Handle cancel action based on current state
        switch (gameManager.currentState)
        {
            case GameManager.GameState.Playing:
                gameManager.PauseGame();
                break;
            case GameManager.GameState.Paused:
                gameManager.ResumeGame();
                break;
            case GameManager.GameState.GameOver:
                gameManager.ReturnToMainMenu();
                break;
        }
    }
}