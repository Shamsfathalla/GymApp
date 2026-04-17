using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Set movement speed in the Inspector

    private Rigidbody2D rb; // Handles the 2D physics and collisions
    private Animator animator; // Handles changing the player's animation states

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Apply joystick movement to the Rigidbody 
        rb.linearVelocity = InputManager.Movement * moveSpeed;
    }

    private void Update()
    {
        Vector2 move = InputManager.Movement;

        // Update animation blend tree based on joystick direction
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.y);

        // Remember the last facing direction when the player lets go of the joystick
        if (move != Vector2.zero)
        {
            animator.SetFloat("LastHorizontal", move.x);
            animator.SetFloat("LastVertical", move.y);
        }
    }
}