using ServiceLocator.UI;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class SubmarineService
    {
        // Private Variables
        private SubmarineConfig submarineConfig;
        private SubmarineView submarineView;

        public SubmarineService(SubmarineConfig _submarineConfig)
        {
            // Setting Variables
            submarineConfig = _submarineConfig;
            submarineView = Object.Instantiate(submarineConfig.submarinePrefab.GetComponent<SubmarineView>());
        }

        public void Init(UIService _uiService)
        {
            // Setting Elements
            submarineView.Init(submarineConfig, _uiService);
        }
        public void FixedUpdate() => submarineView.FixedUpdateSub();
        public void Update() => submarineView.UpdateSub();
        public SubmarineView GetController() => submarineView;
    }
}