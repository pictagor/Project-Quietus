using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStatusEffect : MonoBehaviour
{
    [Header("Stun")]
    public bool isStun;
    public int stun_Counter;
    [SerializeField] TextMeshProUGUI stunText;

    public static EnemyStatusEffect instance;

    private void Awake()
    {
        instance = this;
    }

    public void ApplyStatusEffect()
    {
        switch (DamageCalculator.instance.combatOutcome)
        {
            case DamageCalculator.CombatOutcome.Missed:

                break;

            default:

                if (PlayerController.instance.currentAction.STUN)
                {
                    InflictSTUN();
                }

                break;
        }
    }

    //========= STUN ===============================================================================================================================

    public void InflictSTUN() 
    {
        if (isStun) { return; }
        isStun = true;

        stun_Counter += PlayerController.instance.currentAction.STUN_Counter;
        stunText.text = stun_Counter.ToString();
        stunText.transform.parent.gameObject.SetActive(true);

        StartCoroutine(STUNCoroutine());
    }

    private IEnumerator STUNCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(BattleCountdown(1f));
            stun_Counter--;
            stunText.text = stun_Counter.ToString();
            if (stun_Counter == 0)
            {
                stunText.transform.parent.gameObject.SetActive(false);
                isStun = false;
                yield break;
            }
        }
    }


    //=========== HELPER =============================================================================================================================

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
