using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject elementsGUI;
    [SerializeField] Animator cameraAnimator;
    [SerializeField] GameObject actorSprites;
    [SerializeField] GameObject recoverVFX;

    [SerializeField] float waitForActionName;

    public string currentActionName;
    public int currentSequence;

    [SerializeField] GameObject combatCanvas;
    [SerializeField] GameObject[] combatSequence;

    public bool pauseSlider;

    public UnityEvent onPrecombatStart;
    public UnityEvent onPrecombatEnd;
    public UnityEvent onCombatStarted;
    public UnityEvent onCombatFinished;

    public static CombatManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CombatMenu.instance.onMenuActive.AddListener(PauseCombatSlider);
    }

    // ========== Called by PlayerController & EnemyController when sliders reach zero ================================================

    public void PlayerTriggerPrecombat(PlayerAction playerAction) 
    {
        PauseCombatSlider();

        currentActionName = playerAction.actionName;
        currentSequence = playerAction.sequenceID;

        StartCoroutine(PlayerPrecombatCamera(playerAction));
        onPrecombatStart.Invoke();
    }

    public void EnemyTriggerPrecombat(EnemyAction enemyAction)
    {
        PauseCombatSlider();

        currentActionName = enemyAction.actionName;
        currentSequence = enemyAction.sequenceID;

        StartCoroutine(EnemyPrecombatCamera(enemyAction));
        onPrecombatStart.Invoke();
    }

    // ========== CAMERA MOVEMENTS ================================================================================================

    private IEnumerator PlayerPrecombatCamera(PlayerAction playerAction)
    {
        elementsGUI.SetActive(false);
        PanCameraLeft();

        if (playerAction.actionType == PlayerAction.ActionType.ShortWait || playerAction.actionType == PlayerAction.ActionType.LongWait)
        {
            recoverVFX.SetActive(true);
        }

        yield return new WaitForSeconds(waitForActionName);
        onPrecombatEnd.Invoke();

        if(playerAction.actionType == PlayerAction.ActionType.Attack)
        {
            StartCombatCamera();
            StartCombatSequence();
        }
        else
        {
            cameraAnimator.SetTrigger("Default");
            ResumeCombatSlider();
            elementsGUI.SetActive(true);
        }
    }

    private IEnumerator EnemyPrecombatCamera(EnemyAction enemyAction)
    {
        elementsGUI.SetActive(false);
        PanCameraRight();

        yield return new WaitForSeconds(waitForActionName);
        onPrecombatEnd.Invoke();

        StartCombatCamera();
        StartCombatSequence();
    }

    public void StartCombatCamera()
    {
        cameraAnimator.SetTrigger("Zoom");
        actorSprites.SetActive(false);
    }

    public void PanCameraLeft()
    {
        cameraAnimator.SetTrigger("PanLeft");
    }

    public void PanCameraRight()
    {
        cameraAnimator.SetTrigger("PanRight");
    }

    // ========== COMBAT SPRITES ================================================================================================

    public void PauseCombatSlider()
    {
        pauseSlider = true;
    }

    public void ResumeCombatSlider()
    {
        pauseSlider = false;
    }

    public void StartCombatSequence()
    {
        combatSequence[currentSequence].SetActive(true);
        onCombatStarted.Invoke();
    }

    public void EndCombatSequence() // Called by Individual CombatSequence
    {
        ResumeCombatSlider();
        foreach (GameObject sequence in combatSequence)
        {
            sequence.SetActive(false);
        }

        onCombatFinished.Invoke();
    }

    // ========== REVEAL - Called by Animator ================================================================================================

    public void RevealActors() 
    {
        elementsGUI.SetActive(true);
        actorSprites.SetActive(true);
    }
}
