using ServiceLocator.Controls;
using ServiceLocator.Player;
using ServiceLocator.Profile;
using ServiceLocator.UI;
using ServiceLocator.Vision;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController
    {
        // Private Services
        private GameService gameService;

        private InputService inputService;
        private CameraService cameraService;
        private UIService uiService;
        private SubmarineService submarineService;
        private ProfileService profileService;

        public GameController(GameService _gameService)
        {
            // Setting Variables
            Time.timeScale = 0f;

            // Setting Services
            gameService = _gameService;

            // Setting Elements
            CreateServices();
            InjectDependencies();

            inputService.GetInputControls().Game.Pause.started += ctx => PauseGame();
        }
        private void CreateServices()
        {
            // Setting Services
            inputService = new InputService();
            cameraService = new CameraService(gameService.cameraConfig, gameService.frontCamera,
                gameService.miniMapCamera, gameService.postProcessingVolume);
            uiService = new UIService(gameService.uiCanvas, this);
            submarineService = new SubmarineService(gameService.submarineConfig);
            profileService = new ProfileService();
        }
        private void InjectDependencies()
        {
            inputService.Init();
            cameraService.Init(inputService, submarineService);
            uiService.Init(profileService);
            submarineService.Init(inputService, uiService);
            profileService.Init(uiService);
        }
        public void FixedUpdate()
        {
            // Input Service
            // Camera Service
            // UI Service
            submarineService.FixedUpdate();
            // Profile Service
        }
        public void Update()
        {
            // Input Service
            // Camera Service
            // UI Service
            submarineService.Update();
            // Profile Service
        }
        public void LateUpdate()
        {
            // Input Service
            cameraService.LateUpdate();
            // UI Service
            // Submarine Service
            // Profile Service
        }
        public void Destroy()
        {
            inputService.Destroy();
            // Camera Service
            uiService.Destroy();
            // Submarine Service
            // Profile Service
        }

        public void PlayGame()
        {
            Time.timeScale = 1f;
            uiService.GetController().mainMenuPanel.gameObject.SetActive(false);
            uiService.GetController().pauseMenuPanel.gameObject.SetActive(false);
            uiService.GetController().profileFormPanel.gameObject.SetActive(false);
            uiService.GetController().controlMenuPanel.gameObject.SetActive(false);
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            uiService.GetController().pauseMenuPanel.gameObject.SetActive(true);
        }
        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
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