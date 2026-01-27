using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("InGameMap1");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Mainmenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit!");
    }
}