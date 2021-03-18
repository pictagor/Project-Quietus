using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] GameObject elementsGUI;
    [SerializeField] Animator cameraAnimator;
    [SerializeField] GameObject actorSprites;
    [SerializeField] GameObject combatSprites;
    [SerializeField] TextMeshProUGUI actionText;

    [SerializeField] float waitForActionName;

    public static CombatCamera instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {

    }

    public void TriggerCombat(string actionName, int sequence)
    {
        StartCoroutine(TriggerCombatCo(actionName, sequence));
    }

    private IEnumerator TriggerCombatCo(string actionName, int sequence)
    {
        CombatSprites.instance.animatingCombat = true;

        elementsGUI.SetActive(false);
        actionText.transform.parent.gameObject.SetActive(true);
        actionText.text = actionName;

        yield return new WaitForSeconds(waitForActionName);

        actorSprites.SetActive(false);
        actionText.transform.parent.gameObject.SetActive(false);

        cameraAnimator.SetTrigger("Zoom");
        CombatSprites.instance.StartCombatSequence(sequence);
    }

    //public void RevealGUI() // Called by Animator
    //{
    //    combatSprites.SetActive(false);
    //}

    public void RevealActors() // Called by Animator
    {
        elementsGUI.SetActive(true);
        actorSprites.SetActive(true);
    }
}
