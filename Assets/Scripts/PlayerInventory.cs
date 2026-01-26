using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] int healingPotionAmount;
    int healAmount = 20;

    public PlayerStats playerStatsInput;

    [Header("(UI)")]
    public TextMeshProUGUI potionText;
    void Start()
    {
        UpdatePotionUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) usePotion("heal");
    }
    public void usePotion(string potionType)
    {
        if (potionType == "heal")
        {
            if (healingPotionAmount >= 1)
            {
                playerStatsInput.Heal(healAmount);
                healingPotionAmount--;

                UpdatePotionUI();
            }
            else { Debug.Log("You dont have Healing Potion"); }
        }
    }

    public void GetHealingPotion()
    {
        healingPotionAmount++;

        Debug.Log($"Healing Potion : {healingPotionAmount}");
        UpdatePotionUI();
    }
    void UpdatePotionUI()
    {
        if (potionText != null)
        {
            potionText.text = "x " + healingPotionAmount.ToString();
        }
    }
}
