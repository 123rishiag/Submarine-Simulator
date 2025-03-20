using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float turnSpeed = 15;
    [SerializeField] private float verticalSpeed = 15f;

    // As the submarine goes deeper, it moves slower due to pressure.
    // More drag at greater depths.
    [Header("Depth Resistance & Water Drag Settings")]
    [SerializeField] private float defaultMaxDepth = 1000f; // Default Max Depth of Sea if seabed not detected
    [SerializeField] private float maxResistance = 0.5f; // Max speed reduction at the max depth of ocean
    [SerializeField] private float waterDrag = 0.98f;

    // Submarine naturally floats if left alone.
    // The deeper it is, the harder it is to float back up
    [Header("Buoyancy Settings")]
    [SerializeField] private float buoyancyForce = 10f;
    [SerializeField] private float minBuoyancyFactor = 0.2f; // Minimum floating strength at max depth

    [Header("Layer Settings")]
    [SerializeField] private LayerMask seabedLayer;

    // Private Variables
    private Rigidbody rb;

    private Vector3 moveDirection;
    private Vector3 verticalDirection;
    private float turnInput;

    private float maxDepth;
    private float resistanceFactor;

    private float currentBuoyancy;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disabling gravity as there is no gravity inside water

        DetectSeabed();
    }

    private void Update()
    {
        CheckMovementInput();
        ApplyDepthResistance(); // Adjusting speed based on depth
    }
    private void FixedUpdate()
    {
        Move();
        ApplyBuoyancy();
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
        resistanceFactor = Mathf.Clamp01(depthFactor) * maxResistance;
    }
    private void Move()
    {
        // Applying forward/backward and vertical movement
        rb.AddForce(moveDirection * moveSpeed * (1f - resistanceFactor), ForceMode.Acceleration);
        rb.AddForce(verticalDirection * verticalSpeed * (1f - resistanceFactor), ForceMode.Acceleration);
        ApplyTurn();
        ApplyDrag();
    }
    private void ApplyBuoyancy()
    {
        float depth = Mathf.Abs(transform.position.y);

        // Reducing buoyancy at greater depths (weaker floating effect)
        float depthFactor = depth / maxDepth;
        float targetBuoyancy = Mathf.Lerp(buoyancyForce, buoyancyForce * minBuoyancyFactor, depthFactor);
        currentBuoyancy = Mathf.Lerp(currentBuoyancy, targetBuoyancy, Time.fixedDeltaTime);

        // Applying buoyancy force
        rb.AddForce(Vector3.up * currentBuoyancy * (1f - resistanceFactor), ForceMode.Force);
    }
    private void ApplyTurn()
    {
        // Rotating smoothly
        float turnAmount = turnInput * turnSpeed * (1f - resistanceFactor) * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
    private void ApplyDrag()
    {
        // Applying manual drag (to stop movement gradually in water when no input is given)
        if (moveDirection == Vector3.zero)
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, waterDrag * Time.fixedDeltaTime);

        if (verticalDirection == Vector3.zero)
            rb.linearVelocity =
                new Vector3(rb.linearVelocity.x,
                Mathf.Lerp(rb.linearVelocity.y, 0, waterDrag * Time.fixedDeltaTime),
                rb.linearVelocity.z);
    }

    private void DetectSeabed()
    {
        // Detecting Seabed using Raycast
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, seabedLayer))
        {
            maxDepth = Mathf.Abs(hit.point.y);
            // Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green, 5000f);
        }
        else
        {
            maxDepth = defaultMaxDepth;
            Debug.LogError("Seabed not found!");
        }
    }
}