using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowTrailParticles; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor"); // Get the layer index for the "Floor" layer
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Play(); // Play the particle system when the player collides with the floor
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor"); // Get the layer index for the "Floor" layer
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Stop(); // Stop the particle system when the player exits the floor
        }
    }

}
