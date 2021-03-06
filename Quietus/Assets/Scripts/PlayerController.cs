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

    [Header("Wait")]
    public float longWaitFactor = 0.002f;
    public float longWaitConst = 0.75f;
    public float maxLongWait = 100;
    public float minlongWait = 25;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CombatManager.instance.onCombatFinished.AddListener(ResetDefendState);
    }

    // ==================== Called by CombatButton when Select ========================================================

    public void SetCurrentAction(PlayerAction playerAction) 
    {
        currentAction = playerAction;
    }

    // ==================== Called by CombatButton when Confirm ========================================================

    public void ChooseCombatAction(PlayerAction playerAction)
    {
        CombatManager.instance.ResumeCombatSlider();
        if (playerAction.actionType == PlayerAction.ActionType.Quickstep)
        {
            Quickstep.instance.SpawnArrows();
        }
        else if (playerAction.useAmmo)
        {
            Aiming.instance.TriggerAim();
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
            else if (CombatManager.instance.pauseSlider || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                combatSlider.value = Mathf.Clamp(combatSlider.value - sliderSpeed, 0, 100);
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    PerformAction();

                    //currentAction = null;
                    CombatMenu.instance.actionQueued = false;

                    yield break;
                }

                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    // ==================== When slider reaches zero ========================================================

    public void PerformAction()
    {
        onActionReady.Invoke();
        switch (currentAction.actionType)
        {
            case PlayerAction.ActionType.Dodge:
                defendState = DefendState.Dodging;
                CalculateDodgeChance();

                break;

            case PlayerAction.ActionType.Block:
                defendState = DefendState.Blocking;
                break;

            case PlayerAction.ActionType.ShortWait:
                defendState = DefendState.Waiting;
                CombatManager.instance.PlayerTriggerPrecombat(currentAction);
                Sanity.instance.RegainSanity();
                break;

            case PlayerAction.ActionType.LongWait:
                defendState = DefendState.Waiting;
                CombatManager.instance.PlayerTriggerPrecombat(currentAction);
                Sanity.instance.RegainSanity();
                break;

            case PlayerAction.ActionType.Attack:
                CombatManager.instance.PlayerTriggerPrecombat(currentAction);
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
        if (currentAction.actionType == PlayerAction.ActionType.LongWait)
        {
            CalculateLongWaitSpeed();
        }
        else
        {
            effectiveSpeed = currentAction.baseSpeed * speedModifier;
        }
    }

    public void CalculateLongWaitSpeed()
    {
        float waitSpeed = Mathf.Pow(Sanity.instance.sanityCounter, 2f) / Mathf.Pow(longWaitConst, 2f) * longWaitFactor;
        effectiveSpeed = Mathf.RoundToInt(Mathf.Clamp(waitSpeed, minlongWait, maxLongWait));
    }

}
