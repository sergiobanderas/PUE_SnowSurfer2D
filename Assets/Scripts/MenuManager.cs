using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");        
    }

    /// <summary>
    /// Loads the characters scene.
    /// </summary>  
    public void GoToCharacters()
    {
        SceneManager.LoadScene("Characters");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
    public void SelectCharacter(int characterIndex)
    {        
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        GoToMenu();
    }

}
