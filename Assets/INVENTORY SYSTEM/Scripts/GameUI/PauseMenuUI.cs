// PauseMenuUI.cs - Pause menu UI controller
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button resumeButton;
    private Button restartButton;
    private Button mainMenuButton;

    [Header("References")]
    public GameManager gameManager;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get button references
        resumeButton = root.Q<Button>("ResumeButton");
        restartButton = root.Q<Button>("RestartButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");

        // Subscribe to button events
        resumeButton.clicked += OnResumeButtonClicked;
        restartButton.clicked += OnRestartButtonClicked;
        mainMenuButton.clicked += OnMainMenuButtonClicked;

        // Hide initially
        HideUI();
    }

    private void OnDestroy()
    {
        if (resumeButton != null)
            resumeButton.clicked -= OnResumeButtonClicked;
        if (restartButton != null)
            restartButton.clicked -= OnRestartButtonClicked;
        if (mainMenuButton != null)
            mainMenuButton.clicked -= OnMainMenuButtonClicked;
    }

    private void OnResumeButtonClicked()
    {
        gameManager.ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
        gameManager.RestartGame();
    }

    private void OnMainMenuButtonClicked()
    {
        gameManager.ReturnToMainMenu();
    }

    public void ShowUI()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void HideUI()
    {
        root.style.display = DisplayStyle.None;
    }
}