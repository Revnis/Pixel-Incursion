using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusWindow : MonoBehaviour
{
    [Header("Main References")]
    public GameObject uiPanel;
    public PlayerStats playerStats;

    [Header("UI Text Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI agiText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pointText;

    bool isOpen = false;

    void Start()
    {
        uiPanel.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWindow();
        }
    }

    public void ToggleWindow()
    {
        isOpen = !isOpen; 
        uiPanel.SetActive(isOpen);

        if (isOpen)
        {
            UpdateUI(); 
        }
        else
        {}
    }

    public void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Lv. " + playerStats.level.ToString();
            expText.text = $"{playerStats.currentExp} / {playerStats.maxExp}";
        }

        strText.text = $"{playerStats.currentStrength}";
        agiText.text = $"{playerStats.currentAgility}";;
        hpText.text = $"{playerStats.maxHP}";
        pointText.text = $"{playerStats.playerCurrentStatsPoint}";
    }

    public void OnClickUpgradeSTR()
    {
        playerStats.UpgradeStat("STR");
        UpdateUI();
    }

    public void OnClickUpgradeAGI()
    {
        playerStats.UpgradeStat("AGI");
        UpdateUI();
    }

    public void OnClickUpgradeHP()
    {
        playerStats.UpgradeStat("HP");
        UpdateUI();
    }
}