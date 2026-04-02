using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // เพิ่มบรรทัดนี้เพื่อใช้งาน InputField ค่ะ

public class MenuController : MonoBehaviour
{
    [Header("Player Name Settings")]
    public TMP_InputField nameInputField;

    private void Start()
    {
        if (nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName", "Player");
        }
    }

    public void GoToGame()
    {
        SavePlayerName();
        SceneManager.LoadScene("InGameMap1");
    }

    public void GoToTutorial()
    {
        SavePlayerName();
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
    private void SavePlayerName()
    {
        if (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.Save();
            Debug.Log($"Player Name : {nameInputField.text}");
        }
    }
}