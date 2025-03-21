using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Other Controllers")]
    [SerializeField] private GameController gameController;
    [SerializeField] private SubmarineController submarineController;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text depthDisplay;

    [Header("Pause Menu Elements")]
    [SerializeField] public GameObject pauseMenuPanel; // Pause Menu Panel
    [SerializeField] public Button pauseMenuResumeButton; // Button to resume game
    [SerializeField] public Button pauseMenuMainMenuButton; // Button to go to main menu

    [Header("Main Menu Elements")]
    [SerializeField] public GameObject mainMenuPanel; // Main Menu Panel
    [SerializeField] public Button mainMenuPlayButton; // Button to start the game
    [SerializeField] public Button mainMenuQuitButton; // Button to quit the game

    private void Start()
    {
        pauseMenuResumeButton.onClick.AddListener(gameController.PlayGame);
        pauseMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

        mainMenuPlayButton.onClick.AddListener(gameController.PlayGame);
        mainMenuQuitButton.onClick.AddListener(gameController.QuitGame);
    }

    private void Update()
    {
        UpdateDepthDisplay();
    }

    private void UpdateDepthDisplay()
    {
        depthDisplay.text = "Current Depth: " + submarineController.DepthFromWaterSurface.ToString();
    }
}
