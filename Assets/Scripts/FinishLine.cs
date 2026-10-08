using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    // Reference to the particle system
    [SerializeField] private ParticleSystem finishParticles; 

    // This method is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WIN!!!");
            //Play Particles    
            finishParticles.Play();

            //Reload Scene after 1.5 second
            Invoke(nameof(ReloadScene), 1.5f); 

        }
    }

    /// <summary>
    /// Reloads the current scene after a delay.
    /// </summary>
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
