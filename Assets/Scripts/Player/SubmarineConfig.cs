using UnityEngine;

namespace ServiceLocator.Player
{
    [CreateAssetMenu(fileName = "SubmarineConfig", menuName = "Scriptable Objects/SubmarineConfig")]
    public class SubmarineConfig : ScriptableObject
    {
        [Header("Prefabs")]
        public SubmarineView submarinePrefab;

        [Header("Movement Settings")]
        public float moveSpeed = 1000f;
        public float turnSpeed = 150;
        public float verticalSpeed = 150f;

        // As the submarine goes deeper, it moves slower due to pressure.
        // More drag at greater depths.
        [Header("Depth Resistance & Water Drag Settings")]
        public Vector3 depthSensorOffset = new Vector3(0f, 1.9f, 0f);
        public float defaultMaxDepth = 1000f; // Default Max Depth of Sea if seabed not detected
        public float maxResistance = 0.5f; // Max speed reduction at the max depth of ocean
        public float waterDrag = 0.98f;

        // Submarine naturally floats if left alone.
        // The deeper it is, the harder it is to float back up
        [Header("Buoyancy Settings")]
        public float buoyancyForce = 5000f;
        public float minBuoyancyFactor = 0.2f; // Minimum floating strength at max depth

        [Header("Layer Settings")]
        public LayerMask waterLayer;
        public LayerMask seabedLayer;
    }
}