using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Slider combatSlider;
    [SerializeField] Slider previewSlider;
    [SerializeField] float sliderInterval;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] GameObject sliderHandle;


    public List<EnemyAction> enemyActionList;
    
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        sliderHandle.SetActive(false);
        yield return new WaitForSeconds(1f);

        ChooseCombatAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ActivateCombatSlider(EnemyAction enemyAction)
    {
        combatSlider.value = enemyAction.baseSpeed;
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
        while (combatSlider.value > 0)
        {
            if (CombatMenu.instance.isMenuActive) { yield return null; }
            else if (CombatSprites.instance.animatingCombat)
            {
                yield return null;
            }
            else
            {
                combatSlider.value -= speed;
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    sliderHandle.SetActive(false);

                    CombatCamera.instance.TriggerCombat(enemyAction.actionName, enemyAction.sequenceID, false);
                    DamageCalculator.instance.CalculatePlayerDamage(enemyAction.baseDamage, enemyAction.baseHitChance);

                    yield return new WaitUntil(() => CombatSprites.instance.animatingCombat == false);
                    ChooseCombatAction();
                    yield break;
                }
                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    private void ChooseCombatAction()
    {
        StartCoroutine(ActivateCombatSlider(enemyActionList[0]));
        sliderHandle.SetActive(true);
    }

    private void ShowPreviewSlider(int actionSpeed)
    {
        previewSlider.gameObject.SetActive(true);
        previewSlider.value = actionSpeed;
    }

    private void HidePreviewSlider()
    {
        previewSlider.gameObject.SetActive(true);
        previewSlider.value = 0;
    }
}
