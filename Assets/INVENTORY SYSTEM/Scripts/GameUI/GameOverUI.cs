// GameOverUI.cs - Game over UI controller
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button restartButton;
    private Button mainMenuButton;
    private Label gameOverLabel;
    private Label scoreLabel;

    [Header("References")]
    public GameManager gameManager;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get UI element references
        restartButton = root.Q<Button>("RestartButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");
        gameOverLabel = root.Q<Label>("GameOverLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");

        // Subscribe to button events
        restartButton.clicked += OnRestartButtonClicked;
        mainMenuButton.clicked += OnMainMenuButtonClicked;

        // Hide initially
        HideUI();
    }

    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.clicked -= OnRestartButtonClicked;
        if (mainMenuButton != null)
            mainMenuButton.clicked -= OnMainMenuButtonClicked;
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

    public void SetScore(int score)
    {
        if (scoreLabel != null)
            scoreLabel.text = $"Score: {score}";
    }

    public void SetGameOverText(string text)
    {
        if (gameOverLabel != null)
            gameOverLabel.text = text;
    }
}