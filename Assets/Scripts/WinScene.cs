using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WinScene : MonoBehaviour
{
    [Header("UI & Settings")]
    public TextMeshProUGUI stageTextUI;
    public float showDuration = 3f;

    [TextArea]
    public List<string> winMessages;

    [Header("Next Level Setup")]
    public string nextSceneName = "MainMenu";

    [Header("Targets")]
    public GameObject[] bossObjects; 

    private bool isGameWon = false; 

    private void Start()
    {
        if (stageTextUI != null) stageTextUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isGameWon) return;

        if (CheckAllBossesDead())
        {
            isGameWon = true;
            StartCoroutine(ShowWinSequence());
        }
    }

    bool CheckAllBossesDead()
    {
        if (bossObjects == null || bossObjects.Length == 0) return false;

        foreach (GameObject boss in bossObjects)
        {
            if (boss != null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator ShowWinSequence()
    {
        Debug.Log("Starting ending sequence...");

        if (stageTextUI != null)
        {
            stageTextUI.gameObject.SetActive(true);

            foreach (string message in winMessages)
            {
                stageTextUI.text = message;
                yield return new WaitForSeconds(showDuration);
            }

            stageTextUI.gameObject.SetActive(false);
        }

        Debug.Log("👋 Loading Main Menu...");
        SceneManager.LoadScene(nextSceneName);
    }
}