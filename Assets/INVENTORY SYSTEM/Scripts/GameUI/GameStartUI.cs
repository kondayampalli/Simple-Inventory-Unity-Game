// GameStartUI.cs - Main menu UI controller
using UnityEngine;
using UnityEngine.UIElements;

public class GameStartUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button startButton;
    private Button exitButton;

    [Header("References")]
    public GameManager gameManager;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Get button references
        startButton = root.Q<Button>("StartButton");
        exitButton = root.Q<Button>("ExitButton");

        // Subscribe to button events
        startButton.clicked += OnStartButtonClicked;
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnDestroy()
    {
        if (startButton != null)
            startButton.clicked -= OnStartButtonClicked;
        if (exitButton != null)
            exitButton.clicked -= OnExitButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        gameManager.StartGame();
    }

    private void OnExitButtonClicked()
    {
        gameManager.ExitGame();
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