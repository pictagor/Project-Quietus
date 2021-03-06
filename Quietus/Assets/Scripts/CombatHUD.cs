using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CombatHUD : MonoBehaviour
{
    [Header("Action Name")]
    [SerializeField] TextMeshProUGUI actionText;

    [Header("Damage Text")]
    [SerializeField] GameObject combatCanvas;
    [SerializeField] TextMeshProUGUI enemyDamage;
    [SerializeField] TextMeshProUGUI playerDamage;
    private float enemy_originY;
    private float player_originY;

    [Header("Player Slider")]
    [SerializeField] Slider previewSlider;
    [SerializeField] GameObject sliderHandle;
    [SerializeField] TextMeshProUGUI counterText;
    public TextMeshProUGUI queueText_Player;
    public TextMeshProUGUI queueText_Enemy;

    public static CombatHUD instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player_originY = playerDamage.rectTransform.anchoredPosition.y;
        enemy_originY = enemyDamage.rectTransform.anchoredPosition.y;

        CombatManager.instance.onPrecombatStart.AddListener(DisplayActionName);
        CombatManager.instance.onPrecombatEnd.AddListener(HideActionName);

        CombatManager.instance.onCombatStarted.AddListener(EnableCombatCanvas);
        CombatManager.instance.onCombatFinished.AddListener(DisableCombatCanvas);

        PlayerController.instance.onActionChosen.AddListener(DisplayPlayerQueueText);
        PlayerController.instance.onActionReady.AddListener(HidePlayerQueueText);
        PlayerController.instance.onActionReady.AddListener(HideSliderHandle);

        EnemyController.instance.onActionChosen.AddListener(DisplayEnemyQueueText);
        EnemyController.instance.onActionReady.AddListener(HideEnemyQueueText);

        sliderHandle.SetActive(false);
    }

    // ================================== ACTION NAME ================================================================

    public void DisplayActionName()
    {
        actionText.transform.parent.gameObject.SetActive(true);
        actionText.text = CombatManager.instance.currentActionName;
    }

    public void HideActionName()
    {
        actionText.text = null;
        actionText.transform.parent.gameObject.SetActive(false);
    }

    // ================================== DAMAGE TEXT ================================================================

    public void EnableCombatCanvas()
    {
        combatCanvas.SetActive(true);
    }

    public void DisableCombatCanvas()
    {
        combatCanvas.SetActive(false);
    }

    public void DisplayEnemyDamage() // Called by Combat Sprites
    {
        switch (DamageCalculator.instance.combatOutcome)
        {

            case DamageCalculator.CombatOutcome.Missed:
                enemyDamage.text = "MISSED";
                enemyDamage.color = Color.white;
                enemyDamage.fontSize = 50;
                break;

            //case DamageCalculator.CombatOutcome.Grazed:
            //    enemyDamage.text = "GRAZED\n" + DamageCalculator.instance.damageDealt.ToString();
            //    enemyDamage.color = Color.red;
            //    enemyDamage.fontSize = 50;
            //    break;

            default:
                if (PlayerController.instance.currentAction.STUN && EnemyController.instance.combatSlider.value > 0)
                {
                    EnemyController.instance.combatSlider.value = 0;
                    enemyDamage.text = "<size=50>STUNNED</size>\n" + DamageCalculator.instance.damageDealt.ToString();
                    enemyDamage.color = Color.red;
                    enemyDamage.fontSize = 100;
                }
                else
                {
                    enemyDamage.text = DamageCalculator.instance.damageDealt.ToString();
                    enemyDamage.color = Color.red;
                    enemyDamage.fontSize = 100;
                }
                break;
        }

        enemyDamage.rectTransform.anchoredPosition = new Vector3(enemyDamage.rectTransform.anchoredPosition.x, enemy_originY);
        enemyDamage.rectTransform.DOAnchorPosY(enemy_originY + 150f, 1.2f);
        enemyDamage.DOFade(0f, 1.2f);
    }

    public void DisplayPlayerDamage() // Called by Combat Sprites
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


    // ================================== QUEUE TEXT & SLIDER HANDLE ================================================================

    public void DisplayPlayerQueueText()
    {
        queueText_Player.text = PlayerController.instance.currentAction.actionName;
        HidePreviewSlider();
    }

    public void HidePlayerQueueText()
    {
        queueText_Player.text = null;
    }

    public void DisplaySliderHandle()
    {
        sliderHandle.SetActive(true);
    }

    public void HideSliderHandle()
    {
        sliderHandle.SetActive(false);
    }

    public void DisplayEnemyQueueText()
    {
        queueText_Enemy.text = EnemyController.instance.currentAction.actionName;
    }

    public void HideEnemyQueueText()
    {
        queueText_Player.text = null;
    }

    // ================================== PREVIEW SLIDER ================================================================

    public void ShowPreviewSlider() // Called by CombatButton
    {
        previewSlider.gameObject.SetActive(true);
        previewSlider.value = PlayerController.instance.effectiveSpeed;

        counterText.text = previewSlider.value.ToString();

        // TESTING DODGE CALCULATION
        //float previewPenalty = baseHitPenalty_Dodge + (EnemyController.instance.combatSlider.value - effectiveSpeed) / 100;
        //dodgePenaltyText.text = previewPenalty.ToString();
    }

    public void HidePreviewSlider()
    {
        previewSlider.value = 0;
        counterText.text = null;
        previewSlider.gameObject.SetActive(false);
    }
}
