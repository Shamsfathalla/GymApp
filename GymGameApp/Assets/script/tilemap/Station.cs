using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    
    private int PlayerCollidersRange = 0; // Tracks how many player colliders are inside the trigger zone

    // Opens the menu only if the player is physically standing withing the proximity of the station
    public void OpenMenu()
    {
        if (PlayerCollidersRange > 0) ToggleMenu(true);
    }

    public void CloseMenu() => ToggleMenu(false); // Close the menu button

    private void ToggleMenu(bool isOpen)
    {
        menuPanel.SetActive(isOpen);
        InputManager.IsMenuOpen = isOpen; // Locks/unlocks the joystick
    }

    // Detects when the player enters the station area using "Player" tag
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other)) PlayerCollidersRange++;
    }

    // Detects when the player leaves the station area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other)) PlayerCollidersRange--;
    }

    // Checks if the object has the "Player" tag 
    private bool IsPlayer(Collider2D other) => 
        other.CompareTag("Player") || other.transform.root.CompareTag("Player");
}