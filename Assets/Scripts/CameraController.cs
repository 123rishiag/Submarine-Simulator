using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Camera frontCamera;
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private Vector3 frontCameraPositionOffset = new Vector3(-0.25f, 1.5f, 5f);
    [SerializeField] private float miniMapCameraPitch = 90f;
    [SerializeField] private float miniMapHeight = 50f; // Height above the submarine
    [SerializeField] private float miniMapSize = 100f; // Adjust the field of view of the mini-map

    [Header("Post Processing Settings")]
    [SerializeField] private Volume postProcessingVolume;

    [Header("Night Vision Settings")]
    [SerializeField] private Color colorAdjustmentColor = new Color(0.3f, 0.8f, 0.3f); // Softer Green
    [SerializeField] private float colorAdjustmentExposure = 1.5f; // Slight Brightness Boost
    [SerializeField] private float colorAdjustmentSaturation = -30f; // Reduce Colors for a more night-vision look

    [SerializeField] private float filmGrainIntensity = 0.2f; // Subtle noise effect
    [SerializeField] private float filmGrainResponse = 0.7f; // Smooth details

    [SerializeField] private float vignetteIntensity = 0.25f; // Adds a slight dark border around the screen
    [SerializeField] private float vignetteSmoothness = 0.5f; // Smooth edges of vignette

    // Private Variables
    private ColorAdjustments colorAdjustments;
    private FilmGrain filmGrainEffect;
    private Vignette vignetteEffect;
    private bool isNightVisionOn = false;

    private void Start()
    {
        InitializeVariables();
    }
    private void InitializeVariables()
    {
        // Setting Minimap Camera Rotation
        miniMapCamera.orthographicSize = miniMapSize;
        miniMapCamera.transform.rotation = Quaternion.Euler(miniMapCameraPitch, 0f, 0f);

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

    private void Update()
    {
        CheckNightVisionInput();
    }
    private void LateUpdate()
    {
        SetCameras();
    }

    private void SetCameras()
    {
        // Front camera should always follow the player
        Vector3 frontCameraPosition = transform.position + transform.TransformDirection(frontCameraPositionOffset);
        frontCamera.transform.position = frontCameraPosition;
        frontCamera.transform.rotation = transform.rotation;

        // Keeping the mini-map camera above the submarine
        Vector3 miniMapCameraPosition = transform.position;
        miniMapCameraPosition.y += miniMapHeight;
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
        colorAdjustments.colorFilter.value = colorAdjustmentColor;
        colorAdjustments.postExposure.value = colorAdjustmentExposure;
        colorAdjustments.saturation.value = colorAdjustmentSaturation;

        // Film Grain Settings
        filmGrainEffect.intensity.value = filmGrainIntensity;
        filmGrainEffect.response.value = filmGrainResponse;

        // Vignette Settings
        vignetteEffect.intensity.value = vignetteIntensity;
        vignetteEffect.smoothness.value = vignetteSmoothness;

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
