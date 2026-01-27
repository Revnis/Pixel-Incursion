using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Audio;

public class PlayerStats : MonoBehaviour
{
    [Header("EXP Base at Level 1 (INT)")]
    [SerializeField] public int level = 1;
    [SerializeField] public int currentExp;
    public int maxExp = 100;
    [SerializeField] public int playerCurrentStatsPoint;

    [Header("Attributes")]
    public float currentStrength = 3f;
    public float currentAgility = 2f;
    public float maxHP = 20f;

    [SerializeField] public float currentHP;

    [Header("StatMultiple")]
    float multiStrength = 1f;
    float multiHP = 2f;

    [Header("Player")]
    public PlayerController2D playerController;
    public DamageDealer playerDamageUp;
    public DamageDealer playerDamageSide;
    public DamageDealer playerDamageDown;

    [Header("(UI)")]
    public Healthbar healthBar;
    public GameOverManager gameOverManager;
    public TextMeshProUGUI levelTextHUD;

    public bool isDead = false;

    public float gameOverDelay = 3f;

    [Header("Visual Effects")]
    SpriteRenderer sr;

    private AudioSource audioSource;
    public AudioClip loseSound;
    public AudioClip playerTakeDamageSound;
    public AudioClip playerLevelUpSound;

    Animator anim;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHP = maxHP;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHP);
        }

        StrengthUpgrade();
        AgilityUpgrade();

        UpdateLevelHUD();

        Debug.Log("HP : "+ currentHP);
        Debug.Log("Level : " +  level);
        Debug.Log("Max EXP : " + maxExp);
        Debug.Log("EXP : " + currentExp);
        Debug.Log("Status Points : " + playerCurrentStatsPoint);
        Debug.Log($"STR : {currentStrength}, AGI : {currentAgility}, HP : {maxHP}");
    }
    private void Update()
    {
        
    }
    public void GainExp(int expAmount)
    {
        currentExp = expAmount + currentExp;
        while (currentExp >= maxExp)
        {
            currentExp = currentExp - maxExp;
            level++;
            playerCurrentStatsPoint = playerCurrentStatsPoint + 2;
            audioSource.PlayOneShot(playerLevelUpSound);
            ExpCalculator();

            UpdateLevelHUD();
        }
    }

    void UpdateLevelHUD()
    {
        if (levelTextHUD != null)
        {
            levelTextHUD.text = "Lv. " + level.ToString();
        }
    }

    public void UpgradeStat(string statName)
    {
        if (playerCurrentStatsPoint >= 1)
        {
            if (statName == "STR")
            {
                currentStrength = currentStrength + multiStrength;

                StrengthUpgrade();

                multiStrength = Mathf.Round(multiStrength * 1.5f);
                Debug.Log($"Multi STR :" + multiStrength);
            }
            else if (statName == "AGI")
            {
                currentAgility++;

                AgilityUpgrade();
            }
            else if (statName == "HP")
            {
                maxHP = maxHP + multiHP;
                currentHP = currentHP + multiHP;

                multiHP = Mathf.Round(multiHP * 1.5f);

                if (healthBar != null)
                {
                    healthBar.SetMaxHealth(maxHP);
                    healthBar.SetHealth(currentHP);
                }

                Debug.Log($"Multi HP :" + multiHP);
            }

            playerCurrentStatsPoint--;
            Debug.Log($"STR : {currentStrength}, AGI : {currentAgility}, HP : {maxHP}");
            Debug.Log("Stat : " + playerCurrentStatsPoint);
        }
        else { Debug.Log("Not enough point");  }
    }

    

    public void ExpCalculator()
    {
        maxExp = Mathf.RoundToInt(maxExp * 1.25f);
    }

    public void TakeDamage(int damageAmout)
    {
        if(isDead) return;

        currentHP = currentHP - damageAmout;
        audioSource.PlayOneShot(playerTakeDamageSound);

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP);
        }

        if (damageAmout > 0)
        {
            StartCoroutine(FlashRedEffect());
        }

        if (currentHP <= 0)
        {
            Die();

            audioSource.PlayOneShot(loseSound);
        }

        Debug.Log($"HP : {currentHP}/{maxHP}");
    }

    IEnumerator FlashRedEffect()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        sr.color = Color.white;
    }

    void StrengthUpgrade()
    {
        float newDamage = currentStrength;

        playerDamageUp.damage = newDamage; playerDamageSide.damage = newDamage; playerDamageDown.damage = newDamage;

        Debug.Log($"Current Damage : {newDamage}");
    }
    void AgilityUpgrade()
    {
        float newSpeed = currentAgility;

        playerController.moveSpeed = newSpeed;

        Debug.Log($"Current Speed : {newSpeed}");
    }

    public void Heal(int healAmount)
    {
        currentHP = Mathf.Clamp(currentHP + healAmount, 0, maxHP);
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP);
        }

        Debug.Log($"HP : {currentHP}");
    }
    void Die()
    {
        anim.Play("Dead");
        isDead = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(ShowGameOverRoutine());
    }
    System.Collections.IEnumerator ShowGameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }
}
