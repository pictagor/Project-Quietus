using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Slider combatSlider;
    [SerializeField] Slider previewSlider;
    [SerializeField] float sliderInterval;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] GameObject sliderHandle;


    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        sliderHandle.SetActive(false);
        CombatMenu.instance.DisplayMenu();
    }

    void Update()
    {

    }

    private IEnumerator ActivateCombatSlider(PlayerAction playerAction)
    {
        combatSlider.value = playerAction.baseSpeed;
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();

        HidePreviewSlider();

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

                    CombatCamera.instance.TriggerCombat(playerAction.actionName, playerAction.sequenceID, true);
                    DamageCalculator.instance.CalculateEnemyDamage(playerAction.baseDamage, playerAction.baseHitChance);
                    CombatMenu.instance.actionQueued = false;

                    //yield return new WaitUntil(() => CombatSprites.instance.animatingCombat == false);
                    //ChooseCombatAction();

                    yield break;
                }

                //yield return 0;
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
    }

    public void HidePreviewSlider()
    {
        previewSlider.value = 0;
        previewSlider.gameObject.SetActive(false);
    }
}
