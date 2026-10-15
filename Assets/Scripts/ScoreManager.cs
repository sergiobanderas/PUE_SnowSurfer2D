using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    /// <summary>
    /// Updates the score display with the given score value. 
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("00000");
    }

}
