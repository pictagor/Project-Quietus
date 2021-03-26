using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public List<EnemyAction> enemyActionList;
    public EnemyAction currentAction;
    
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
            else if (CombatSprites.instance.pauseSlider || RallyRing.instance.isRallyActive)
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
                    intent.SetActive(false);

                    CombatCamera.instance.TriggerPrecombat(enemyAction.actionName, enemyAction.sequenceID, false);
                    DamageCalculator.instance.DeterminePlayerFate(effectiveDamage, enemyAction.baseHitChance);

                    yield return new WaitUntil(() => CombatSprites.instance.pauseSlider == false);
                    ChooseCombatAction();
                    yield break;
                }
                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    private void ChooseCombatAction()
    {
        float randomAction = Random.Range(0, 1f);

        currentAction = enemyActionList[0];

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
    }

}
