using UnityEngine;
using UnityEngine.SceneManagement;


public class CrashDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem crashParticles;

    [SerializeField] private AudioClip crashSound; 

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {        
        int layer = LayerMask.NameToLayer("Floor");

        if (other.gameObject.layer == layer)
        {
            Debug.Log("CRASH!!!");
            
            //Play Particles
            crashParticles.Play();

            //Play Sound
            audioSource.PlayOneShot(crashSound);

            //Reload Scene after 1.5 second
            Invoke(nameof(ReloadScene), 1.5f); 
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }

}
