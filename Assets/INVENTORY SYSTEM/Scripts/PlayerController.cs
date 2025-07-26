// 5. PLAYER CONTROLLER
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Interaction")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer = 1;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Camera playerCamera;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
        playerCamera = Camera.main;

        // Subscribe to input events
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.ToggleInventory.performed += OnToggleInventory;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        HandleMovement();
        HandleGravity();

        // Update Animator "Speed" parameter
        if (animator != null)
        {
            // Use the magnitude of moveInput for speed (0 when idle, up to 1 when moving)
            animator.SetFloat("Speed", moveInput.magnitude);
        }
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.magnitude > 0.01f)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            // Smoothly rotate the player towards the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        move = transform.TransformDirection(Vector3.forward); // Always move in the facing direction
        controller.Move(move * moveSpeed * moveInput.magnitude * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryInteract();
        }
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Toggle Inventory performed");
        if (context.performed)
        {
            InventoryUI.Instance?.ToggleInventory();
        }
    }

    private void TryInteract()
    {
        // Ray starts at the player's position and goes forward from the player's transform
        Vector3 origin = transform.position + Vector3.up * 1.0f; // Slightly above the ground (adjust height as needed)
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * interactionRange, Color.red, 10.0f);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactionRange, interactableLayer))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            interactable?.Interact(this);
        }
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.ToggleInventory.performed -= OnToggleInventory;
    }

}