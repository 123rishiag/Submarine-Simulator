using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Other Controllers")]
    [SerializeField] private SubmarineController submarineController;

    [Header("Depth Sensor Settings")]
    [SerializeField] private TMP_Text depthDisplay;

    private void Update()
    {
        UpdateDepthDisplay();
    }

    private void UpdateDepthDisplay()
    {
        depthDisplay.text = "Current Depth: " + submarineController.DepthFromWaterSurface.ToString();
    }
}
