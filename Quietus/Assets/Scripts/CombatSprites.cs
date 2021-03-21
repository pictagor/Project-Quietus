using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CombatSprites : MonoBehaviour
{
    [SerializeField] GameObject combatCanvas;
    [SerializeField] GameObject[] combatSequence;

    [SerializeField] TextMeshProUGUI enemyDamage;
    [SerializeField] TextMeshProUGUI playerDamage;

    private float enemy_originY;
    private float player_originY;
    public bool animatingCombat;

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

    public void EndCombatSequence()
    {
        animatingCombat = false;
        combatCanvas.SetActive(false);
        foreach (GameObject sequence in combatSequence)
        {
            sequence.SetActive(false);
        }
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
        if (DamageCalculator.instance.missed)
        {
            enemyDamage.color = Color.white;
            enemyDamage.fontSize = 50;
        }
        else
        {

            enemyDamage.color = Color.red;
            enemyDamage.fontSize = 100;
        }

        //enemyDamage.color = new Color(enemyDamage.color.r, enemyDamage.color.g, enemyDamage.color.b, 255);
        enemyDamage.rectTransform.anchoredPosition = new Vector3(enemyDamage.rectTransform.anchoredPosition.x, enemy_originY);
        enemyDamage.rectTransform.DOAnchorPosY(enemy_originY + 150f, 1.2f);
        enemyDamage.DOFade(0f, 1.2f);
    }

    public void DisplayPlayerDamage()
    {
        if (DamageCalculator.instance.missed)
        {
            playerDamage.color = Color.white;
            playerDamage.fontSize = 50;
        }
        else
        {

            playerDamage.color = Color.red;
            playerDamage.fontSize = 100;
        }

        //playerDamage.color = new Color(playerDamage.color.r, playerDamage.color.g, playerDamage.color.b, 255);
        playerDamage.rectTransform.anchoredPosition = new Vector3(playerDamage.rectTransform.anchoredPosition.x, player_originY);
        playerDamage.rectTransform.DOAnchorPosY(player_originY + 150f, 1.2f);
        playerDamage.DOFade(0f, 1.2f);
    }
}
