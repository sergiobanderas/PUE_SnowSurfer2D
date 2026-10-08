using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // This method is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WIN!!!");
        }
    }
}
