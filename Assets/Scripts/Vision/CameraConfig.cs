using UnityEngine;

namespace ServiceLocator.Vision
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Scriptable Objects/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Camera References")]
        public Vector3 frontCameraPositionOffset = new Vector3(-0.25f, 1.5f, 5f);
        public float miniMapCameraPitch = 90f;
        public float miniMapHeight = 50f; // Height above the submarine
        public float miniMapSize = 100f; // Adjust the field of view of the mini-map

        [Header("Night Vision Settings")]
        public Color colorAdjustmentColor = new Color(0.3f, 0.8f, 0.3f); // Softer Green
        public float colorAdjustmentExposure = 1.5f; // Slight Brightness Boost
        public float colorAdjustmentSaturation = -30f; // Reduce Colors for a more night-vision look

        public float filmGrainIntensity = 0.2f; // Subtle noise effect
        public float filmGrainResponse = 0.7f; // Smooth details

        public float vignetteIntensity = 0.25f; // Adds a slight dark border around the screen
        public float vignetteSmoothness = 0.5f; // Smooth edges of vignette
    }
}