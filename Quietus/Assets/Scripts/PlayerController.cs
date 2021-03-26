using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    public enum DefendState
    {
        None,
        Dodging,
        Blocking,
        Quickstepping,
        Waiting
    }
    public DefendState defendState;

    [Header("Events")]
    public UnityEvent onActionChosen;
    public UnityEvent onActionReady;
    public PlayerAction currentAction;

    [Header("Slider")]
    public Slider combatSlider;
    public float sliderInterval;
    [SerializeField] float sliderSpeed;
    [SerializeField] GameObject sliderHandle;
    [SerializeField] TextMeshProUGUI counterText;

    [Header("Speed")]
    public float effectiveSpeed;
    public float speedModifier = 1f;

    [Header("Dodge")]
    public float baseHitPenalty_Dodge;
    public float hitPenalty_Dodge;

    [Header("Block")]
    public int baseBlockDamage;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CombatSprites.instance.onCombatFinished.AddListener(ResetDefendState);
    }

    // ==================== Called by CombatButton when Select ========================================================

    public void SetCurrentAction(PlayerAction playerAction) 
    {
        currentAction = playerAction;
    }

    // ==================== Called by CombatButton when Confirm ========================================================

    public void ChooseCombatAction(PlayerAction playerAction)
    {
        if (playerAction.actionType == PlayerAction.ActionType.Quickstep)
        {
            Quickstep.instance.SpawnArrows();
        }
        else
        {
            if (playerAction.baseSpeed > 0)
            {
                StartCoroutine(ActivateCombatSlider());
                CombatHUD.instance.DisplaySliderHandle();
                CombatMenu.instance.actionQueued = true;
            }
        }
        onActionChosen.Invoke();
    }

    // ==================== Slider Behavior ========================================================

    private IEnumerator ActivateCombatSlider()
    {
        combatSlider.value = effectiveSpeed;
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();

        RallyRing.instance.ChanceForRallyRing();

        while (combatSlider.value >= 0)
        {
            if (CombatMenu.instance.isMenuActive) { yield return null; }
            else if (CombatSprites.instance.pauseSlider || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                combatSlider.value = Mathf.Clamp(combatSlider.value - sliderSpeed, 0, 100);
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    onActionReady.Invoke();
                    PerformAction();

                    currentAction = null;
                    CombatMenu.instance.actionQueued = false;

                    yield break;
                }

                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    // ==================== When slider reaches zero ========================================================

    private void PerformAction()
    {
        switch (currentAction.actionType)
        {
            case PlayerAction.ActionType.Dodge:
                defendState = DefendState.Dodging;
                CalculateDodgeChance();

                break;

            case PlayerAction.ActionType.Block:
                defendState = DefendState.Blocking;
                break;

            case PlayerAction.ActionType.Wait:
                defendState = DefendState.Waiting;
                Sanity.instance.RegainSanity();
                break;

            case PlayerAction.ActionType.Attack:
                CombatCamera.instance.TriggerPrecombat(currentAction.actionName, currentAction.sequenceID, true);
                DamageCalculator.instance.DetermineEnemyFate(currentAction.baseDamage, currentAction.baseHitChance);
                break;
        }
    }


    // ==================== Other Calculations ========================================================

    public void ResetDefendState()
    {
        defendState = DefendState.None;
    }

    private void CalculateDodgeChance()
    {
        hitPenalty_Dodge = baseHitPenalty_Dodge + EnemyController.instance.combatSlider.value / 100;
    }

    public void CalculateEffectiveSpeed()
    {
        effectiveSpeed = currentAction.baseSpeed * speedModifier;
    }

}
