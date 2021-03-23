using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class CombatSprites : MonoBehaviour
{
    [SerializeField] GameObject combatCanvas;
    [SerializeField] GameObject[] combatSequence;

    [SerializeField] TextMeshProUGUI enemyDamage;
    [SerializeField] TextMeshProUGUI playerDamage;

    private float enemy_originY;
    private float player_originY;
    public bool animatingCombat;

    public UnityEvent onCombatFinished;

    public static CombatSprites instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player_originY = playerDamage.rectTransform.anchoredPosition.y;
        enemy_originY = enemyDamage.rectTransform.anchoredPosition.y;
    }

    public void StartCombatSequence(int index)
    {
        combatSequence[index].SetActive(true);
        combatCanvas.SetActive(true);
    }

    public void EndCombatSequence() // Called by Individual CombatSequence
    {
        animatingCombat = false;
        combatCanvas.SetActive(false);
        foreach (GameObject sequence in combatSequence)
        {
            sequence.SetActive(false);
        }

        onCombatFinished.Invoke();
    }

    public void DamageToPlayer(int dmg)
    {
        playerDamage.text = dmg.ToString();
    }

    public void DamageToEnemy(int dmg)
    {
        enemyDamage.text = dmg.ToString();
    }


    public void DisplayEnemyDamage()
    {
        switch(DamageCalculator.instance.combatOutcome)
        {

            case DamageCalculator.CombatOutcome.Missed:
                enemyDamage.text = "MISSED";
                enemyDamage.color = Color.white;
                enemyDamage.fontSize = 50;
                break;

            case DamageCalculator.CombatOutcome.Grazed:
                enemyDamage.text = "GRAZED\n" + DamageCalculator.instance.damageDealt.ToString();
                enemyDamage.color = Color.red;
                enemyDamage.fontSize = 50;
                break;

            default:
                enemyDamage.text = DamageCalculator.instance.damageDealt.ToString();
                enemyDamage.color = Color.red;
                enemyDamage.fontSize = 100;
                break;
        }

        enemyDamage.rectTransform.anchoredPosition = new Vector3(enemyDamage.rectTransform.anchoredPosition.x, enemy_originY);
        enemyDamage.rectTransform.DOAnchorPosY(enemy_originY + 150f, 1.2f);
        enemyDamage.DOFade(0f, 1.2f);
    }

    public void DisplayPlayerDamage()
    {
        switch (DamageCalculator.instance.combatOutcome)
        {

            case DamageCalculator.CombatOutcome.Missed:
                playerDamage.text = "MISSED";
                playerDamage.color = Color.white;
                playerDamage.fontSize = 50;
                break;

            case DamageCalculator.CombatOutcome.Grazed:
                playerDamage.text = "GRAZED\n" + DamageCalculator.instance.damageDealt.ToString();
                playerDamage.color = Color.red;
                playerDamage.fontSize = 50;
                break;

            default:
                playerDamage.text = DamageCalculator.instance.damageDealt.ToString();
                playerDamage.color = Color.red;
                playerDamage.fontSize = 100;
                break;
        }

        //playerDamage.color = new Color(playerDamage.color.r, playerDamage.color.g, playerDamage.color.b, 255);
        playerDamage.rectTransform.anchoredPosition = new Vector3(playerDamage.rectTransform.anchoredPosition.x, player_originY);
        playerDamage.rectTransform.DOAnchorPosY(player_originY + 150f, 1.2f);
        playerDamage.DOFade(0f, 1.2f);
    }
}
