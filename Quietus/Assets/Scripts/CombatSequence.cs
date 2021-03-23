using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CombatSequence : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] GameObject bloodSprite;
    private Material playerMat;
    private Material enemyMat;

    private void Awake()
    {
        playerMat = playerSprite.material;
        enemyMat = enemySprite.material;
    }

    private void FinishCombatAnimation()
    {
        CombatSprites.instance.EndCombatSequence();
    }

    private void ShowEnemyDamage()
    {
        CombatSprites.instance.DisplayEnemyDamage();
    }

    private void ShowPlayerDamage()
    {
        CombatSprites.instance.DisplayPlayerDamage();
    }

    private void EnemySpriteEffect()
    {
        switch (DamageCalculator.instance.combatOutcome)
        {

            case DamageCalculator.CombatOutcome.Missed:
                bloodSprite.SetActive(false);
                break;

            case DamageCalculator.CombatOutcome.Grazed:

                break;

            case DamageCalculator.CombatOutcome.Hit:
                EnemyFlashHit();
                bloodSprite.SetActive(true);
                break;

        }
    }

    private void PlayerSpriteEffect()
    {
        switch (DamageCalculator.instance.combatOutcome)
        {
            case DamageCalculator.CombatOutcome.Missed:
                bloodSprite.SetActive(false);
                break;

            case DamageCalculator.CombatOutcome.Grazed:

                break;

            case DamageCalculator.CombatOutcome.Hit:
                PlayerFlashHit();
                bloodSprite.SetActive(true);
                break;

        }
    }

    public void PlayerFlashHit()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(playerMat.DOFloat(0.4f, "_HitEffectBlend", 0.1f));
        mySequence.Append(playerMat.DOFloat(0f, "_HitEffectBlend", 0.7f));
    }

    public void EnemyFlashHit()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(enemyMat.DOFloat(0.4f, "_HitEffectBlend", 0.1f));
        mySequence.Append(enemyMat.DOFloat(0f, "_HitEffectBlend", 0.7f));
    }
}
