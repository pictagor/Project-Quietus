using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DamageCalculator : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI enemyDamage;
    [SerializeField] TextMeshProUGUI playerDamage;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image enemyHealthBar;

    public int maxHealth_Player;
    public int currentHealth_Player;
    public int maxHealth_Enemy;
    public int currentHealth_Enemy;

    public bool missed;

    public static DamageCalculator instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth_Player = maxHealth_Player;
        currentHealth_Enemy = maxHealth_Enemy;
    }

    public void CalculatePlayerDamage(int damage, float hitChance) // Called by PlayerController
    {
        // Determine Hit or Missed
        float hit = Random.Range(0, 1f);
        if (hit < hitChance) { missed = false; }
        else { missed = true; }

        // Update Damage Text and Player Health
        if (missed)
        {
            playerDamage.text = "MISSED";
        }
        else
        {
            playerDamage.text = damage.ToString();

            currentHealth_Player = Mathf.Clamp(currentHealth_Player - damage, 0, maxHealth_Player);
            UpdatePlayerHealth();
        }
    }

    public void CalculateEnemyDamage(int damage, float hitChance) // Called by EnemyController
    {
        // Determine Hit or Missed
        float hit = Random.Range(0, 1f);
        if (hit < hitChance) { missed = false; }
        else { missed = true; }

        // Update Damage Text and Enemy Health
        if (missed)
        {
            enemyDamage.text = "MISSED";
        }
        else
        {
            enemyDamage.text = damage.ToString();

            currentHealth_Enemy = Mathf.Clamp(currentHealth_Enemy - damage, 0, maxHealth_Enemy);
            UpdateEnemyHealth();
        }
    }

    public void UpdatePlayerHealth()
    {
        playerHealthBar.fillAmount = (float) currentHealth_Player / (float) maxHealth_Player;
    }

    public void UpdateEnemyHealth()
    {
        enemyHealthBar.fillAmount = (float) currentHealth_Enemy / (float) maxHealth_Enemy;
    }
}
