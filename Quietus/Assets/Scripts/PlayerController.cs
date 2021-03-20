using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Slider combatSlider;
    [SerializeField] float sliderInterval;
    [SerializeField] float speed;
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
        counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
        while (combatSlider.value > 0)
        {
            if (CombatCamera.instance.isMenu) { yield return 0; }
            if (CombatSprites.instance.animatingCombat)
            {
                yield return null;
            }
            else
            {
                combatSlider.value -= speed;
                counterText.text = Mathf.FloorToInt(combatSlider.value).ToString();
                if (combatSlider.value == 0)
                {
                    CombatCamera.instance.TriggerCombat(actionName, sequence, true);
                    yield return new WaitUntil(() => CombatSprites.instance.animatingCombat == false);
                    //ChooseCombatAction();
                    yield break;
                }

                //yield return 0;
                yield return new WaitForSeconds(sliderInterval);
            }
        }
    }

    private void ChooseCombatAction()
    {
        // Vicious Strike
        StartCoroutine(ActivateCombatSlider(40, 1, "Melee Strike"));
    }

    /// PLAYER MOVE /////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    
    public void MeleeAttack_1()
    {
        StartCoroutine(ActivateCombatSlider(40, 0, "Heavy Slash"));
    }

    public void RangedAttack_1()
    {
        StartCoroutine(ActivateCombatSlider(35, 1, "Triple Shots"));
    }
}
