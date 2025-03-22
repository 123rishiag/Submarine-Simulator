using ServiceLocator.UI;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class SubmarineView : MonoBehaviour
    {
        // Private Variables
        private SubmarineConfig submarineConfig;
        private Rigidbody rb;

        private Vector3 moveDirection;
        private Vector3 verticalDirection;
        private float turnInput;

        private Vector3 depthSensorPosition;
        private float maxDepth;
        private float resistanceFactor;

        private float currentBuoyancy;

        private float waterSurfacePositonY;
        public float DepthFromWaterSurface { get; private set; }

        // Private Services
        private UIService uiService;

        public void Init(SubmarineConfig _submarineConfig, UIService _uiService)
        {
            // Setting Variables
            submarineConfig = _submarineConfig;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false; // Disabling gravity as there is no gravity inside water

            // Setting Services
            uiService = _uiService;

            DetectWaterSurface();
            DetectSeabed();
        }
        private void DetectWaterSurface()
        {
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = Vector3.up;

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, submarineConfig.waterLayer))
            {
                waterSurfacePositonY = hit.point.y;
                // Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green, 5000f);
            }
            else
            {
                Debug.LogError("Water Surface not found!");
            }
        }
        private void DetectSeabed()
        {
            // Detecting Seabed using Raycast
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = Vector3.down;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, submarineConfig.seabedLayer))
            {
                maxDepth = Mathf.Abs(hit.point.y);
                // Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green, 5000f);
            }
            else
            {
                maxDepth = submarineConfig.defaultMaxDepth;
                Debug.LogError("Seabed not found!");
            }
        }
        public void FixedUpdateSub()
        {
            Move();
            ApplyBuoyancy();
        }
        public void UpdateSub()
        {
            CheckMovementInput();
            ApplyDepthResistance(); // Adjusting speed based on depth
            DetectDepthFromWaterSurface();
        }
        private void CheckMovementInput()
        {
            // Calculating move Input
            float moveInput = Input.GetAxis("Vertical");
            moveDirection = transform.forward * moveInput;

            // Calculating turn input
            turnInput = Input.GetAxis("Horizontal");
            if (Vector3.Dot(transform.forward, moveDirection) < 0)
                turnInput = turnInput * -1; // Reversing Turn movements when going backwards

            // Calculating vertical input
            verticalDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.Space))
                verticalDirection = Vector3.up;
            else if (Input.GetKey(KeyCode.LeftControl))
                verticalDirection = Vector3.down;
        }
        private void ApplyDepthResistance()
        {
            float depth = Mathf.Abs(transform.position.y);

            float depthFactor = depth / maxDepth;
            // Fetching Resistance Fatctor based on MaxResistance
            resistanceFactor = Mathf.Clamp01(depthFactor) * submarineConfig.maxResistance;
        }
        private void DetectDepthFromWaterSurface()
        {
            depthSensorPosition = transform.position + submarineConfig.depthSensorOffset;
            DepthFromWaterSurface = Mathf.Max(0, waterSurfacePositonY - depthSensorPosition.y);
            uiService.GetController().UpdateDepthDisplayUI(DepthFromWaterSurface);
        }
        private void Move()
        {
            // Applying forward/backward and vertical movement
            rb.AddForce(moveDirection * submarineConfig.moveSpeed * (1f - resistanceFactor), ForceMode.Acceleration);
            rb.AddForce(verticalDirection * submarineConfig.verticalSpeed * (1f - resistanceFactor), ForceMode.Acceleration);
            ApplyTurn();
            ApplyDrag();
        }
        private void ApplyBuoyancy()
        {
            float depth = Mathf.Abs(transform.position.y);

            // Reducing buoyancy at greater depths (weaker floating effect)
            float depthFactor = depth / maxDepth;
            float targetBuoyancy = Mathf.Lerp(
                submarineConfig.buoyancyForce, submarineConfig.buoyancyForce * submarineConfig.minBuoyancyFactor, depthFactor);
            currentBuoyancy = Mathf.Lerp(currentBuoyancy, targetBuoyancy, Time.fixedDeltaTime);

            // Applying buoyancy force
            rb.AddForce(Vector3.up * currentBuoyancy * (1f - resistanceFactor), ForceMode.Force);
        }
        private void ApplyTurn()
        {
            // Rotating smoothly
            float turnAmount = turnInput * submarineConfig.turnSpeed * (1f - resistanceFactor) * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
        private void ApplyDrag()
        {
            // Applying manual drag (to stop movement gradually in water when no input is given)
            if (moveDirection == Vector3.zero)
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, submarineConfig.waterDrag * Time.fixedDeltaTime);

            if (verticalDirection == Vector3.zero)
                rb.linearVelocity =
                    new Vector3(rb.linearVelocity.x,
                    Mathf.Lerp(rb.linearVelocity.y, 0, submarineConfig.waterDrag * Time.fixedDeltaTime),
                    rb.linearVelocity.z);
        }

        // Getters
        public Transform GetTransform() => transform;
    }
}