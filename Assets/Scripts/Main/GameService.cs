using ServiceLocator.Player;
using ServiceLocator.UI;
using ServiceLocator.Vision;
using UnityEngine;
using UnityEngine.Rendering;

namespace ServiceLocator.Main
{
    public class GameService : MonoBehaviour
    {
        [Header("UI Variables")]
        [SerializeField] public UIView uiCanvas;

        [Header("Camera Variables")]
        [SerializeField] public CameraConfig cameraConfig;
        [SerializeField] public Camera frontCamera;
        [SerializeField] public Camera miniMapCamera;
        [SerializeField] public Volume postProcessingVolume;

        [Header("Game Variables")]
        [SerializeField] public SubmarineConfig submarineConfig;

        // Private Variables
        private GameController gameController;

        private void Start() => gameController = new GameController(this);
        private void FixedUpdate() => gameController.FixedUpdate();
        private void Update() => gameController.Update();
        private void LateUpdate() => gameController.LateUpdate();
        private void OnDestroy() => gameController.Destroy();
    }
}