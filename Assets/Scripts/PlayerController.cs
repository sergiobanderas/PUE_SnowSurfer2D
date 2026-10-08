using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float boostMultiplier = 30f; // Multiplier for the boost speed
    [SerializeField] private float baseSpeed = 15f; 

    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector2D;

    InputAction moveAction;
    Vector2 moveValue ; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Move action from the Input System
        moveAction = InputSystem.actions.FindAction("Move");
    
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindAnyObjectByType<SurfaceEffector2D>();           
    
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        BoostPlayer();
    }

    private void BoostPlayer()
    {
        // if the player is moving  (up), increase the speed of the SurfaceEffector2D
        if (moveValue.y > 0)
        {
            surfaceEffector2D.speed = boostMultiplier; // Increase speed when moving forward
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed; // Reset speed when not moving forward
        }

    }


    private void RotatePlayer()
    {
        // Read the value of the Move action and store it in moveValue
        moveValue = moveAction.ReadValue<Vector2>();

        if (moveValue.x > 0)
        {
            rb.AddTorque(-5f); // Apply torque to rotate the player clockwise
        }
        else if (moveValue.x < 0)
        {
            rb.AddTorque(5f); // Apply torque to rotate the player counterclockwise
        }
    }
}
