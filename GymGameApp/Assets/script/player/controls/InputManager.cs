using UnityEngine;
using UnityEngine.InputSystem; 

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement; 
    public static bool IsMenuOpen;

    private PlayerInput playerInput;
    private Camera mainCamera; 

    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>(); 
        mainCamera = Camera.main; // Cache the main camera reference for later use
    }

    private void Update()
    {
        // 1. MOVEMENT CHECK 
        // We removed the lock! The player can now move freely even if a menu is open.
        Movement = playerInput.actions["Move"].ReadValue<Vector2>();

        // 2. CLICK/TAP CHECK
        if (playerInput.actions["Click"].WasPressedThisFrame())
        {
            HandleTap();
        }
    }

    private void HandleTap()
    {
        Vector2 screenPos = playerInput.actions["Point"].ReadValue<Vector2>();  // Get the screen position of the tap/click
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(screenPos);  // Convert screen position to world position

        Collider2D hit = Physics2D.OverlapPoint(worldPos); // Check if we hit any collider at the tap position
        
        if (hit != null) // If we hit something, check if it's a station and try to open the menu
        {
            Station station = hit.GetComponentInParent<Station>(); 
            
            if (station != null)
            {
                station.OpenMenu(); 
            }
        }
    }
}