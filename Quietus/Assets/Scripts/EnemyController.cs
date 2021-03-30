using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public Slider combatSlider;
    [SerializeField] float sliderInterval;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] GameObject sliderHandle;
    [SerializeField] GameObject intent;
    [SerializeField] TextMeshProUGUI intentText;

    private float speedRoll;
    public int effectiveDamage;

    public List<EnemyAction> baseActionList;
    public List<EnemyAction> availableActionList;
    public EnemyAction currentAction;

    public UnityEvent onActionChosen;
    public UnityEvent onActionReady;
    
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        sliderHandle.SetActive(false);
        intent.SetActive(false);

        for (int i = 0; i < baseActionList.Count; i++)
        {
            availableActionList.Add(baseActionList[i]);
        }

        yield return new WaitForSeconds(1f);
        ChooseCombatAction();
    }


    private IEnumerator ActivateCombatSlider(EnemyAction enemyAction)
    {
        combatSlider.value = speedRoll;
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
        while (combatSlider.value >= 0)
        {
            if (CombatMenu.instance.isMenuActive) { yield return null; }
            else if (CombatManager.instance.pauseSlider || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                combatSlider.value = Mathf.Clamp(combatSlider.value - speed, 0, 100);
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    onActionReady.Invoke();
                    sliderHandle.SetActive(false);
                    intent.SetActive(false);

                    CombatManager.instance.EnemyTriggerPrecombat(enemyAction);
                    DamageCalculator.instance.DeterminePlayerFate(effectiveDamage, enemyAction);

                    yield return new WaitUntil(() => CombatManager.instance.pauseSlider == false);
                    ChooseCombatAction();
                    yield break;
                }
                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    private void ChooseCombatAction()
    {
        int randomAction = Random.Range(0, availableActionList.Count);

        // Start action cooldown (if applicable)
        currentAction = availableActionList[randomAction];
        if (currentAction.cooldown > 0)
        {
            StartCoroutine(TriggerActionCooldown(currentAction.cooldown));
        }

        // Speed Roll
        speedRoll = Random.Range(currentAction.minSpeed, currentAction.maxSpeed);

        // Damage Calculation
        float factor_a = (currentAction.maxDamage - currentAction.minDamage) / (currentAction.maxSpeed - currentAction.minSpeed);
        float factor_b = currentAction.maxDamage - factor_a * currentAction.maxSpeed;
        effectiveDamage = Mathf.RoundToInt(factor_a * speedRoll + factor_b); 

        StartCoroutine(ActivateCombatSlider(currentAction));
        sliderHandle.SetActive(true);

        // Display Intent
        intent.SetActive(true);
        intentText.text = effectiveDamage.ToString();

        // Invoke Event
        onActionChosen.Invoke();
    }


    private IEnumerator TriggerActionCooldown(float cooldownDuration)
    {
        EnemyAction myAction = currentAction;
        availableActionList.Remove(myAction);
        yield return StartCoroutine(BattleCountdown(cooldownDuration));
        availableActionList.Add(myAction);
    }


    /// =========== HELPER ======================================================================================

    public static IEnumerator BattleCountdown(float duration)
    {
        float timer = 0;
        while (true)
        {
            if (CombatMenu.instance.isMenuActive || CombatManager.instance.pauseSlider || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= duration)
                {
                    yield break;
                }
            }
            yield return null;
        }
    }
}
