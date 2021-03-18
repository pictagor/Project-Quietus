using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSequence : MonoBehaviour
{
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
}
