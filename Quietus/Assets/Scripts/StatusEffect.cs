﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffect : MonoBehaviour
{
    [Header("Speed Boost")]
    public float speedModifier;

    [Header("Doom")]
    public bool isDoom;
    public int doom_Counter;
    public int doom_maxCounter = 3;
    [SerializeField] TextMeshProUGUI doomText;

    [Header("Poison")]
    public bool isPoison;
    public int poison_DamagePerTick;
    public int poison_Counter;
    public float poison_Interval = 1f;
    [SerializeField] TextMeshProUGUI poisonText;


    public static StatusEffect instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CombatMenu.instance.onMenuActive.AddListener(CountdownDOOM);
    }

    public void ApplyStatusEffect()
    {
        switch (DamageCalculator.instance.combatOutcome)
        {
            case DamageCalculator.CombatOutcome.Missed:

                break;

            default:

                if (EnemyController.instance.currentAction.POISON)
                {
                    InflicPOISON();
                }

                if (EnemyController.instance.currentAction.POISON)
                {

                }
                break;
        }
    }

    //========= DOOM ===============================================================================================================================

    public void InflictDOOM() // Called by Sanity
    {
        if (isDoom) { return; }
        isDoom = true;
        doom_Counter = doom_maxCounter;
        doomText.transform.parent.gameObject.SetActive(true);
        doomText.text = doom_Counter.ToString();
    }

    public void RemoveDOOM()
    {
        isDoom = false;
        doomText.transform.parent.gameObject.SetActive(false);
    }

    public void CountdownDOOM()
    {
        if (!isDoom) { return; }
        doom_Counter--;
        doomText.text = doom_Counter.ToString();
        if (doom_Counter == 0)
        {
            Debug.Log("GAME OVER!");
        }
    }


    //========= POISON =============================================================================================================================

    public void InflicPOISON()
    {
        poison_Counter += EnemyController.instance.currentAction.POISON_Counter;
        poisonText.text = poison_Counter.ToString();
        poison_DamagePerTick = EnemyController.instance.currentAction.POISON_damagePerTick;

        poisonText.transform.parent.gameObject.SetActive(true);
        StartCoroutine(PoisonDamageOverTime());
    }

    private IEnumerator PoisonDamageOverTime()
    {
        if (isPoison) { yield break; }

        float timer = 0;

        poison_Interval = 1f;
        isPoison = true;

        while(true)
        {
            if (CombatMenu.instance.isMenuActive || CombatManager.instance.pauseSlider || RallyRing.instance.isRallyActive)
            {
                yield return null;
            }
            else
            {
                DamageCalculator.instance.PlayerTakeDamage(poison_DamagePerTick);

                timer += 0.1f;
                //timer += Time.deltaTime;
                if (timer >= poison_Interval)
                {                
                    timer = 0f;
                    poison_Counter--;
                    poisonText.text = poison_Counter.ToString();

                    if (poison_Counter == 0)
                    {
                        poisonText.transform.parent.gameObject.SetActive(false);
                        isPoison = false;
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


}
