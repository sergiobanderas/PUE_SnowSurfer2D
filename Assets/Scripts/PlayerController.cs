using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

    InputAction moveAction;
    Vector2 moveValue ; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Move action from the Input System
        moveAction = InputSystem.actions.FindAction("Move");
    
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
