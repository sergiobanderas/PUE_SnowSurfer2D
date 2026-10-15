using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float boostMultiplier = 30f; // Multiplier for the boost speed
    [SerializeField] private float baseSpeed = 15f; 
    [SerializeField] private ScoreManager scoreManager; 

    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector2D;

    InputAction moveAction;
    Vector2 moveValue ; 
    int flipCount;
    float totalRotation;
    float previousRotation;

    float maxTorque = 500f;


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
        CalculateFlips();
    }

    private void CalculateFlips()
    {
        float currentRotation = transform.rotation.eulerAngles.z; 
        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation); 
        if (Mathf.Abs(totalRotation) >= 340f ) 
        {
            flipCount++;
            totalRotation = 0f;            
            scoreManager.UpdateScore(flipCount*100); // Update the score based on flips
        }
        previousRotation = currentRotation;
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
            rb.AddTorque(-2f); // Apply torque to rotate the player clockwise
            if (rb.angularVelocity < -maxTorque)
            {
                rb.angularVelocity = -maxTorque; // Limit the maximum clockwise rotation speed
            }
        }
        else if (moveValue.x < 0)
        {
            rb.AddTorque(2f); // Apply torque to rotate the player counterclockwise
            if (rb.angularVelocity > maxTorque)
            {
                rb.angularVelocity = maxTorque; // Limit the maximum counterclockwise rotation speed
            }
        }
    }
}
