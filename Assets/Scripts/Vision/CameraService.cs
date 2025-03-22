using ServiceLocator.Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ServiceLocator.Vision
{
    public class CameraService
    {
        // Private Variables
        private CameraConfig cameraConfig;
        private Camera frontCamera;
        private Camera miniMapCamera;
        private Volume postProcessingVolume;

        private ColorAdjustments colorAdjustments;
        private FilmGrain filmGrainEffect;
        private Vignette vignetteEffect;
        private bool isNightVisionOn = false;

        // Private Services
        private SubmarineService submarineService;

        public CameraService(CameraConfig _cameraConfig, Camera _frontCamera, Camera _miniMapCamera, Volume _postProcessingVolume)
        {
            // Setting Variables
            cameraConfig = _cameraConfig;
            frontCamera = _frontCamera;
            miniMapCamera = _miniMapCamera;
            postProcessingVolume = _postProcessingVolume;

            // Setting Elements
            miniMapCamera.orthographicSize = cameraConfig.miniMapSize;
            miniMapCamera.transform.rotation = Quaternion.Euler(cameraConfig.miniMapCameraPitch, 0f, 0f);

            // Setting Postprocessing Variables
            if (postProcessingVolume.profile.TryGet(out colorAdjustments) &&
                postProcessingVolume.profile.TryGet(out filmGrainEffect) &&
                postProcessingVolume.profile.TryGet(out vignetteEffect))
            {
                DisableNightVision();
            }
            else
            {
                Debug.LogError("Post Processing Effects missing in Volume Profile!");
            }
        }
        public void Init(SubmarineService _submarineService)
        {
            // Setting Services
            submarineService = _submarineService;
        }
        public void Update()
        {
            CheckNightVisionInput();
        }
        public void LateUpdate()
        {
            FollowCameras();
        }

        private void FollowCameras()
        {
            Transform submarineTransform = submarineService.GetController().GetTransform();

            // Front camera should always follow the player
            Vector3 frontCameraPosition = submarineTransform.position +
                submarineTransform.TransformDirection(cameraConfig.frontCameraPositionOffset);
            frontCamera.transform.position = frontCameraPosition;
            frontCamera.transform.rotation = submarineTransform.rotation;

            // Keeping the mini-map camera above the submarine
            Vector3 miniMapCameraPosition = submarineTransform.position;
            miniMapCameraPosition.y += cameraConfig.miniMapHeight;
            miniMapCamera.transform.position = miniMapCameraPosition;
        }

        private void CheckNightVisionInput()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                isNightVisionOn = !isNightVisionOn;
                if (isNightVisionOn)
                    EnableNightVision();
                else
                    DisableNightVision();
            }
        }
        private void EnableNightVision()
        {
            // Color Adjustments Settings
            colorAdjustments.colorFilter.value = cameraConfig.colorAdjustmentColor;
            colorAdjustments.postExposure.value = cameraConfig.colorAdjustmentExposure;
            colorAdjustments.saturation.value = cameraConfig.colorAdjustmentSaturation;

            // Film Grain Settings
            filmGrainEffect.intensity.value = cameraConfig.filmGrainIntensity;
            filmGrainEffect.response.value = cameraConfig.filmGrainResponse;

            // Vignette Settings
            vignetteEffect.intensity.value = cameraConfig.vignetteIntensity;
            vignetteEffect.smoothness.value = cameraConfig.vignetteSmoothness;

            // Shadow Settings
            frontCamera.GetUniversalAdditionalCameraData().renderShadows = false;
        }
        private void DisableNightVision()
        {
            // Color Adjustments Settings
            colorAdjustments.colorFilter.value = Color.white;
            colorAdjustments.postExposure.value = 0f;
            colorAdjustments.saturation.value = 0f;

            // Film Grain Settings
            filmGrainEffect.intensity.value = 0f;
            filmGrainEffect.response.value = 0f;

            // Vignette Settings
            vignetteEffect.intensity.value = 0f;
            vignetteEffect.smoothness.value = 0f;

            // Shadow Settings
            frontCamera.GetUniversalAdditionalCameraData().renderShadows = true;
        }
    }
}