using ServiceLocator.Player;
using ServiceLocator.Profile;
using ServiceLocator.UI;
using ServiceLocator.Vision;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController
    {
        // Private Variables
        private bool isPaused;

        // Private Services
        private GameService gameService;

        //private InputService inputService;
        private CameraService cameraService;
        private UIService uiService;
        private SubmarineService submarineService;
        private ProfileService profileService;

        public GameController(GameService _gameService)
        {
            // Setting Variables
            isPaused = true;

            // Setting Services
            gameService = _gameService;

            // Setting Elements
            CreateServices();
            InjectDependencies();
        }
        private void CreateServices()
        {
            // Setting Services
            cameraService = new CameraService(gameService.cameraConfig, gameService.frontCamera,
                gameService.miniMapCamera, gameService.postProcessingVolume);
            uiService = new UIService(gameService.uiCanvas, this);
            submarineService = new SubmarineService(gameService.submarineConfig);
            profileService = new ProfileService();
        }
        private void InjectDependencies()
        {
            cameraService.Init(submarineService);
            uiService.Init(profileService);
            submarineService.Init(uiService);
            profileService.Init(uiService);
        }
        public void FixedUpdate()
        {
            submarineService.FixedUpdate();
        }
        public void Update()
        {
            cameraService.Update();
            submarineService.Update();
            PauseGame();

            if (!isPaused)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
        public void LateUpdate()
        {
            cameraService.LateUpdate();
        }
        public void Destroy()
        {
            uiService.Destroy();
        }

        public void PlayGame()
        {
            isPaused = false;
            uiService.GetController().mainMenuPanel.gameObject.SetActive(false);
            uiService.GetController().pauseMenuPanel.gameObject.SetActive(false);
            uiService.GetController().profileFormPanel.gameObject.SetActive(false);
            uiService.GetController().controlMenuPanel.gameObject.SetActive(false);
        }

        private void PauseGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
                uiService.GetController().pauseMenuPanel.gameObject.SetActive(true);
            }
        }
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        public void ProfileMenu()
        {
            uiService.GetController().mainMenuPanel.gameObject.SetActive(false);
            uiService.GetController().profileFormPanel.gameObject.SetActive(true);
        }

        public void EnableControlMenu()
        {
            uiService.GetController().mainMenuPanel.gameObject.SetActive(false);
            uiService.GetController().controlMenuPanel.gameObject.SetActive(true);
        }

        public void DisableControlMenu()
        {
            uiService.GetController().mainMenuPanel.gameObject.SetActive(true);
            uiService.GetController().controlMenuPanel.gameObject.SetActive(false);
        }
    }
}