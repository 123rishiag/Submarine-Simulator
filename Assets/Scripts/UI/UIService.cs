using ServiceLocator.Main;
using ServiceLocator.Profile;

namespace ServiceLocator.UI
{
    public class UIService
    {
        // Private Variables
        private UIView uiCanvas;
        private GameController gameController;

        public UIService(UIView _uiCanvas, GameController _gameController)
        {
            // Setting Variables
            uiCanvas = _uiCanvas.GetComponent<UIView>();
            gameController = _gameController;
        }
        public void Init(ProfileService _profileService)
        {
            uiCanvas.Init(gameController, _profileService);
        }
        public void Destroy()
        {
            uiCanvas.Destroy();
        }

        // Getters
        public UIView GetController() => uiCanvas;
    }
}