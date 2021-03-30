using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class DamageCalculator : MonoBehaviour
{
    public enum CombatOutcome
    {
        Hit,
        Missed, 
        Grazed,
        Critical,
        Blocked
    }
    public CombatOutcome combatOutcome;

    [SerializeField] TextMeshProUGUI enemyDamage;
    [SerializeField] TextMeshProUGUI playerDamage;
    [SerializeField] TextMeshProUGUI enemyHealthCounter;
    [SerializeField] TextMeshProUGUI playerHealthCounter;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image enemyHealthBar;

    private float missRoll;
    public int damageDealt;
    private float finalHitChance;

    public int maxHealth_Player;
    public int currentHealth_Player;
    public int maxHealth_Enemy;
    public int currentHealth_Enemy;

    public static DamageCalculator instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth_Player = maxHealth_Player;
        currentHealth_Enemy = maxHealth_Enemy;

        UpdatePlayerHealth();
        UpdateEnemyHealth();
    }

    //////////////////////////////////////////////////////////////
    ///  ENEMY ATTACKING PLAYER //////////////////////////////////
    //////////////////////////////////////////////////////////////

    public void DeterminePlayerFate(int damage, EnemyAction enemyAction) // Called by EnemyController
    {
        // Determine Combat Outcome
        CheckPlayerFate(enemyAction.baseHitChance);

        // Calculate Damage
        CalculateDamageToPlayer(damage);

        // Apply Status Effect
        StatusEffect.instance.ApplyStatusEffect();
    }

    private void CheckPlayerFate(float hitChance)
    {
        // Roll for Miss/Hit
        missRoll = Random.Range(0, 1f);

        switch (PlayerController.instance.defendState)
        {
            case PlayerController.DefendState.Dodging:
                finalHitChance = Mathf.Clamp(hitChance - PlayerController.instance.hitPenalty_Dodge, 0, 1);
                if (missRoll > finalHitChance)
                {
                    combatOutcome = CombatOutcome.Missed;
                }
                else
                {
                    combatOutcome = CombatOutcome.Grazed;
                }
                break;

            case PlayerController.DefendState.Blocking:
                combatOutcome = CombatOutcome.Blocked;
                break;

            case PlayerController.DefendState.Quickstepping:
                combatOutcome = CombatOutcome.Missed;
                break;

            case PlayerController.DefendState.None:
                finalHitChance = hitChance;
                if (missRoll > finalHitChance)
                {
                    combatOutcome = CombatOutcome.Missed;
                }
                else
                {
                    combatOutcome = CombatOutcome.Hit;
                }              
                break;
        }

        Debug.Log("ROLL:" + missRoll);
        Debug.Log("MISS IF HIGHER THAN: " + finalHitChance + "(" + hitChance + " - " + PlayerController.instance.hitPenalty_Dodge + ")");
    }

    private void CalculateDamageToPlayer(int effectiveDamage)
    {
        switch (combatOutcome)
        {
            case CombatOutcome.Missed:
        
                break;

            case CombatOutcome.Grazed:
                damageDealt = Mathf.Clamp(Mathf.RoundToInt(effectiveDamage * (finalHitChance - missRoll)/finalHitChance), 1, effectiveDamage);
                PlayerTakeDamage(damageDealt);
                break;

            case CombatOutcome.Blocked:
                damageDealt = Mathf.Clamp(effectiveDamage - PlayerController.instance.baseBlockDamage, 1, effectiveDamage);
                PlayerTakeDamage(damageDealt);
                break;

            case CombatOutcome.Hit:
                damageDealt = effectiveDamage;
                PlayerTakeDamage(damageDealt);
                break;
        }
    }

    public void UpdatePlayerHealth()
    {
        playerHealthBar.fillAmount = (float)currentHealth_Player / (float)maxHealth_Player;
        playerHealthCounter.text = currentHealth_Player.ToString() + "/" + maxHealth_Player.ToString();
    }

    public void PlayerTakeDamage(int dmg)
    {
        currentHealth_Player = Mathf.Clamp(currentHealth_Player - dmg, 0, maxHealth_Player);
        UpdatePlayerHealth();
    }

    //////////////////////////////////////////////////////////////
    ///  PLAYER ATTACKING ENEMY //////////////////////////////////
    //////////////////////////////////////////////////////////////

    public void DetermineEnemyFate(int damage, float hitChance) // Called by PlayerController
    {
        CheckEnemyFate(hitChance);

        CalculateDamageToEnemy(damage);
    }

    private void CheckEnemyFate(float hitChance)
    {
        // Determine Hit or Missed
        missRoll = Random.Range(0, 1f);

        if (missRoll > hitChance)
        {
            combatOutcome = CombatOutcome.Missed;
        }
        else
        {
            combatOutcome = CombatOutcome.Hit;
        }
    }

    private void CalculateDamageToEnemy(int damage)
    {
        // Update Damage Text and Enemy Health
        switch (combatOutcome)
        {

            case CombatOutcome.Missed:

                break;

            case CombatOutcome.Grazed:

                break;

            case CombatOutcome.Hit:

                damageDealt = damage;
                EnemyTakeDamage(damageDealt);
                break;
        }
    }

    public void EnemyTakeDamage(int dmg)
    {
        currentHealth_Enemy = Mathf.Clamp(currentHealth_Enemy - damageDealt, 0, maxHealth_Enemy);
        UpdateEnemyHealth();
    }

    public void UpdateEnemyHealth()
    {
        enemyHealthBar.fillAmount = (float) currentHealth_Enemy / (float) maxHealth_Enemy;
        enemyHealthCounter.text = currentHealth_Enemy.ToString() + "/" + maxHealth_Enemy.ToString();
    }
}
