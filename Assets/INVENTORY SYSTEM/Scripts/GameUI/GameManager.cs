// GameManager.cs - Main game state controller
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameStartUI gameStartUI;
    public PauseMenuUI pauseMenuUI;
    public GameOverUI gameOverUI;

    [Header("Input")]
    public InputActionReference pauseAction;

    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Restart,
        Paused,
        GameOver
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameState currentState { get; private set; }

    private void Start()
    {
        SetGameState(GameState.MainMenu);

        // Subscribe to input events
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }
    }

    private void OnDestroy()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                gameStartUI.ShowUI();
                pauseMenuUI.HideUI();
                gameOverUI.HideUI();
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                gameStartUI.HideUI();
                pauseMenuUI.HideUI();
                gameOverUI.HideUI();
                break;

            case GameState.Restart:
                Time.timeScale = 1f;
                SceneManager.LoadScene("Inventory Scene");
                gameStartUI.ShowUI();
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                pauseMenuUI.ShowUI();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                gameOverUI.ShowUI();
                break;
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (currentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }

    // Public methods for UI callbacks
    public void StartGame()
    {
        SetGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }

    public void RestartGame()
    {
        // Reset your game logic here
        SetGameState(GameState.Restart);
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void ReturnToMainMenu()
    {
        SetGameState(GameState.MainMenu);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
