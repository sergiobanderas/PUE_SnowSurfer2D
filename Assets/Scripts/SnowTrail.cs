using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowTrailParticles; 

    private Animator animator;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);

        animator = transform.GetChild(0).GetChild(selectedCharacter).GetComponent<Animator>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor"); // Get the layer index for the "Floor" layer
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Play(); // Play the particle system when the player collides with the floor
            animator.SetBool("isJumping", false); 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        int layer = LayerMask.NameToLayer("Floor"); // Get the layer index for the "Floor" layer
        if (collision.gameObject.layer == layer)
        {
            snowTrailParticles.Stop(); // Stop the particle system when the player exits the floor
            animator.SetBool("isJumping", true);              
        }
    }

}
