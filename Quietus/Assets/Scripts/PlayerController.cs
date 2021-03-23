using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Slider combatSlider;
    [SerializeField] Slider previewSlider;
    public float sliderInterval;
    [SerializeField] float speed;

    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] TextMeshProUGUI queueText;
    [SerializeField] GameObject sliderHandle;

    public enum DefendState
    {
        None,
        Dodging,
        Blocking,
        Quickstep_LEFT,
        Quickstep_RIGHT,
        Quickstep_BACK
    }

    public DefendState defendState;

    public float baseHitPenalty_Dodge;
    public float hitPenalty_Dodge;
    public int baseBlockDamage;

    [SerializeField] TextMeshProUGUI dodgePenaltyText;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sliderHandle.SetActive(false);

        CombatSprites.instance.onCombatFinished.AddListener(ResetDefendState);
    }


    private IEnumerator ActivateCombatSlider(PlayerAction playerAction)
    {
        combatSlider.value = playerAction.baseSpeed;
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
        queueText.text = playerAction.actionName;

        HidePreviewSlider();
        RallyRing.instance.ChanceForRallyRing();

        while (combatSlider.value >= 0)
        {
            if (CombatMenu.instance.isMenuActive) { yield return null; }
            else if (CombatSprites.instance.animatingCombat || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                combatSlider.value = Mathf.Clamp(combatSlider.value - speed, 0, 100);
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    sliderHandle.SetActive(false);
                    queueText.text = null;

                    switch (playerAction.actionName)
                    {
                        case "Dodge":
                            defendState = DefendState.Dodging;
                            CalculateDodgeChance();

                            break;

                        case "Block":                  
                            defendState = DefendState.Blocking;
                            break;

                        default:
                            CombatCamera.instance.TriggerCombat(playerAction.actionName, playerAction.sequenceID, true);
                            DamageCalculator.instance.DetermineEnemyFate(playerAction.baseDamage, playerAction.baseHitChance);
                            break;
                    }

                    CombatMenu.instance.actionQueued = false;

                    yield break;
                }

                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }


    public void ChooseCombatAction(PlayerAction playerAction) // Called by CombatButton
    {
        StartCoroutine(ActivateCombatSlider(playerAction));
        sliderHandle.SetActive(true);
    }

    public void ShowPreviewSlider(int actionSpeed) // Called by CombatButton
    {
        previewSlider.gameObject.SetActive(true);
        previewSlider.value = actionSpeed;

        counterText.text = Mathf.FloorToInt(actionSpeed).ToString();

        // TESTING DODGE CALCULATION
        float previewPenalty = baseHitPenalty_Dodge + (EnemyController.instance.combatSlider.value - actionSpeed) / 100;
        dodgePenaltyText.text = previewPenalty.ToString();

    }

    public void HidePreviewSlider()
    {
        previewSlider.value = 0;
        counterText.text = null;
        previewSlider.gameObject.SetActive(false);
    }

    public void ResetDefendState()
    {
        defendState = DefendState.None;
    }

    private void CalculateDodgeChance()
    {
        hitPenalty_Dodge = baseHitPenalty_Dodge + EnemyController.instance.combatSlider.value / 100;
    }
}
