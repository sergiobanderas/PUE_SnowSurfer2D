using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.5f; 
    [SerializeField] private float scaleDuration = 2.5f;
    [SerializeField] private float animationDuration = 0.15f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //disable the collider and sprite renderer of the power-up object
            GetComponent<Collider2D>().enabled = false; 
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            
            //Start the scaling coroutine
            StartCoroutine(ScalePlayer(other.transform));            
        }        
    }    

    private IEnumerator ScalePlayer(Transform player)
    {
        Vector3 originalScale = player.localScale;
        Vector3 bigScale = originalScale * scaleMultiplier;

        float elapsedTime = 0f;

        // PHASE 1: Grow quickly
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            player.localScale = Vector3.Lerp(
                originalScale, bigScale, t);

            yield return null;
        }

        player.localScale = bigScale;

        // PHASE 2: Keep the increased size
        yield return new WaitForSeconds(scaleDuration);

        // PHASE 3: Return to original size
        elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            player.localScale = Vector3.Lerp(
                bigScale, originalScale, t);

            yield return null;
        }

        player.localScale = originalScale;

        
    }    




}
