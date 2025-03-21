using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GameController gameController;
    [SerializeField] private SubmarineController submarineController;
    [SerializeField] private ProfileManager profileManager;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text depthDisplay;

    [Header("Pause Menu Elements")]
    [SerializeField] public Transform pauseMenuPanel; // Pause Menu Panel
    [SerializeField] public Button pauseMenuResumeButton; // Button to resume game
    [SerializeField] public Button pauseMenuMainMenuButton; // Button to go to main menu

    [Header("Main Menu Elements")]
    [SerializeField] public Transform mainMenuPanel; // Main Menu Panel
    [SerializeField] public Button mainMenuPlayButton; // Button to start the game
    [SerializeField] public Button mainMenuProfileButton; // Button to start the game
    [SerializeField] public Button mainMenuQuitButton; // Button to quit the game

    [Header("Profile Form Elements")]
    [SerializeField] public Transform profileFormPanel;
    [SerializeField] public GameObject profileListItemPrefab;
    [SerializeField] public Transform profileListPanel;
    [SerializeField] public Transform profileListContentPanel;
    [SerializeField] public Transform profileInputPanel;

    [SerializeField] public TMP_InputField profileFirstNameField;
    [SerializeField] public TMP_InputField profileLastNameField;
    [SerializeField] public TMP_InputField profileLocationField;
    [SerializeField] public TMP_InputField profileEmailField;

    [SerializeField] public Button profileSaveButton;
    [SerializeField] public Button profileDeleteButton;
    [SerializeField] public Button profileBackButton;
    [SerializeField] public Button profileNewButton;
    [SerializeField] public Button profileMainMenuButton;

    private void Start()
    {
        pauseMenuResumeButton.onClick.AddListener(gameController.PlayGame);
        pauseMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

        mainMenuPlayButton.onClick.AddListener(gameController.PlayGame);
        mainMenuQuitButton.onClick.AddListener(gameController.QuitGame);

        mainMenuProfileButton.onClick.AddListener(gameController.ProfileMenu);

        profileSaveButton.onClick.AddListener(profileManager.SaveProfile);
        profileDeleteButton.onClick.AddListener(profileManager.DeleteProfile);
        profileBackButton.onClick.AddListener(() => EnableProfileForm(false));
        profileNewButton.onClick.AddListener(profileManager.NewProfile);
        profileMainMenuButton.onClick.AddListener(gameController.MainMenu);


        EnableProfileForm(false);
    }

    private void Update()
    {
        UpdateDepthDisplay();
    }

    private void UpdateDepthDisplay()
    {
        depthDisplay.text = "Current Depth: " + submarineController.DepthFromWaterSurface.ToString();
    }

    public void EnableProfileForm(bool _flag)
    {
        if (_flag)
        {
            profileInputPanel.gameObject.SetActive(true);
            profileListPanel.gameObject.SetActive(false);

            profileSaveButton.gameObject.SetActive(true);
            profileDeleteButton.gameObject.SetActive(true);
            profileBackButton.gameObject.SetActive(true);
            profileNewButton.gameObject.SetActive(false);
            profileMainMenuButton.gameObject.SetActive(false);

        }
        else
        {
            profileInputPanel.gameObject.SetActive(false);
            profileListPanel.gameObject.SetActive(true);

            profileSaveButton.gameObject.SetActive(false);
            profileDeleteButton.gameObject.SetActive(false);
            profileBackButton.gameObject.SetActive(false);
            profileNewButton.gameObject.SetActive(true);
            profileMainMenuButton.gameObject.SetActive(true);
        }
    }
}
