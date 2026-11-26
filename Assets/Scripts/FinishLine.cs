using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{

    // Reference to the particle system
    [SerializeField] private ParticleSystem finishParticles; 
    
    [SerializeField] private AudioClip finishSound; 

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // This method is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WIN!!!");
            //Play Particles    
            finishParticles.Play();

            //Play Sound
            audioSource.PlayOneShot(finishSound);

            //Reload Scene after 1.5 second
            Invoke(nameof(NextLevel), 1.5f); 

        }
    }

    /// <summary>
    /// Loads the next scene in the build settings.
    /// </summary>
    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


    /// <summary>
    /// Reloads the current scene after a delay.
    /// </summary>
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
