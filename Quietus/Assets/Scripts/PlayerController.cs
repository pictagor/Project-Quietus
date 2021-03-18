using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Slider combatSlider;
    [SerializeField] float sliderSpeed;
    [SerializeField] TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(ActivateCombatSlider(40, 0, "Heavy Slash"));
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(ActivateCombatSlider(35, 1, "Triple Shots"));
        }
    }

    private IEnumerator ActivateCombatSlider(int actionSpeed, int sequence, string actionName)
    {
        combatSlider.value = actionSpeed;
        counterText.text = combatSlider.value.ToString();
        while (combatSlider.value > 0)
        {
            if (CombatSprites.instance.animatingCombat)
            {
                yield return null;
            }
            else
            {
                combatSlider.value--;
                counterText.text = combatSlider.value.ToString();
                if (combatSlider.value == 0)
                {
                    CombatCamera.instance.TriggerCombat(actionName, sequence);
                    yield return new WaitUntil(() => CombatSprites.instance.animatingCombat == false);
                    //ChooseCombatAction();
                    yield break;
                }
                yield return new WaitForSeconds(sliderSpeed);
            }
        }
    }

    private void ChooseCombatAction()
    {
        // Vicious Strike
        StartCoroutine(ActivateCombatSlider(40, 1, "Melee Strike"));
    }


}
