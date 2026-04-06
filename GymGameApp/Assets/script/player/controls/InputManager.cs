using UnityEngine;
using UnityEngine.InputSystem; // Unity input system for handling player input

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement; // Stores the current X and Y movement input from the joystick/keyboard
    public static bool IsMenuOpen;

    private PlayerInput playerInput;
    private Camera mainCamera; // The main camera, used to translate screen taps to game world coordinates

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Grab the PlayerInput
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerInput.actions["Click"].performed += HandleTap; // Subscribe to the "Click" action.
    }

    private void OnDisable()
    {
        playerInput.actions["Click"].performed -= HandleTap; //Unsubscribe from the "Click" action.
    }

    private void Update()
    {
        // Stop reading the joystick if a menu is open
        if (IsMenuOpen)
        {
            Movement = Vector2.zero;
        }
        else
        {
            Movement = playerInput.actions["Move"].ReadValue<Vector2>();
        }
    }

    private void HandleTap(InputAction.CallbackContext context)
    {
        if (IsMenuOpen) return; // If a menu is currently open, ignore the tap completely and stop running this code

        // Get touch position and convert to 2D world space
        Vector2 screenPos = playerInput.actions["Point"].ReadValue<Vector2>();
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        // Check if the player tapped a 2D collider
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null)
        {
            // GetComponentInParent checks the tapped object AND its parents automatically
            Station station = hit.GetComponentInParent<Station>();
            
            if (station != null)
            {
                station.OpenMenu(); // Tell that specific station to open its menu
            }
        }
    }
}