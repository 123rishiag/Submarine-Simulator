using ServiceLocator.Main;
using ServiceLocator.Profile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ServiceLocator.UI
{
    public class UIView : MonoBehaviour
    {
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
        [SerializeField] public Button mainMenuControlButton; // Button to start the game
        [SerializeField] public Button mainMenuQuitButton; // Button to quit the game

        [Header("Control Menu Elements")]
        [SerializeField] public Transform controlMenuPanel;
        [SerializeField] public Button controlMenuBackButton;

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

        [SerializeField] public TMP_Text validationText;

        [SerializeField] public Button profileSaveButton;
        [SerializeField] public Button profileDeleteButton;
        [SerializeField] public Button profileBackButton;
        [SerializeField] public Button profileNewButton;
        [SerializeField] public Button profileMainMenuButton;

        // Private Variables
        private GameController gameController;

        // Private Services
        private ProfileService profileService;

        public void Init(GameController _gameController, ProfileService _profileService)
        {
            // Setting Variables
            gameController = _gameController;

            // Setting Services
            profileService = _profileService;

            // Adding Listeners
            pauseMenuResumeButton.onClick.AddListener(gameController.PlayGame);
            pauseMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

            mainMenuPlayButton.onClick.AddListener(gameController.PlayGame);
            mainMenuProfileButton.onClick.AddListener(gameController.ProfileMenu);
            mainMenuControlButton.onClick.AddListener(gameController.EnableControlMenu);
            mainMenuQuitButton.onClick.AddListener(gameController.QuitGame);

            controlMenuBackButton.onClick.AddListener(gameController.DisableControlMenu);

            profileSaveButton.onClick.AddListener(profileService.SaveProfile);
            profileDeleteButton.onClick.AddListener(profileService.DeleteProfile);
            profileBackButton.onClick.AddListener(() => EnableProfileForm(false));
            profileNewButton.onClick.AddListener(profileService.NewProfile);
            profileMainMenuButton.onClick.AddListener(gameController.MainMenu);

            EnableProfileForm(false);
        }

        public void Destroy()
        {
            // Removing Listeners
            pauseMenuResumeButton.onClick.RemoveListener(gameController.PlayGame);
            pauseMenuMainMenuButton.onClick.RemoveListener(gameController.MainMenu);

            mainMenuPlayButton.onClick.RemoveListener(gameController.PlayGame);
            mainMenuProfileButton.onClick.RemoveListener(gameController.ProfileMenu);
            mainMenuControlButton.onClick.RemoveListener(gameController.EnableControlMenu);
            mainMenuQuitButton.onClick.RemoveListener(gameController.QuitGame);

            controlMenuBackButton.onClick.RemoveListener(gameController.DisableControlMenu);

            profileSaveButton.onClick.RemoveListener(profileService.SaveProfile);
            profileDeleteButton.onClick.RemoveListener(profileService.DeleteProfile);
            profileBackButton.onClick.RemoveListener(() => EnableProfileForm(false));
            profileNewButton.onClick.RemoveListener(profileService.NewProfile);
            profileMainMenuButton.onClick.RemoveListener(gameController.MainMenu);
        }

        public void UpdateDepthDisplayUI(float _depth)
        {
            depthDisplay.text = "Current Depth: " + _depth.ToString("F1");
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

                validationText.text = "";

                profileSaveButton.gameObject.SetActive(false);
                profileDeleteButton.gameObject.SetActive(false);
                profileBackButton.gameObject.SetActive(false);
                profileNewButton.gameObject.SetActive(true);
                profileMainMenuButton.gameObject.SetActive(true);
            }
        }
        public void ShowValidationMessage(string _message)
        {
            validationText.text = _message;
        }

    }
}