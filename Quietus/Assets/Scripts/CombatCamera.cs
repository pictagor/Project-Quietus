using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CombatCamera : MonoBehaviour
{
    [SerializeField] GameObject elementsGUI;
    [SerializeField] Animator cameraAnimator;
    [SerializeField] GameObject actorSprites;
    [SerializeField] TextMeshProUGUI actionText;

    [SerializeField] float waitForActionName;

    public string currentActionName;
    public int currentSequence;

    public UnityEvent onPrecombatStart;
    public UnityEvent onPrecombatEnd;

    public static CombatCamera instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    // ========== Called by PlayerController & EnemyController when sliders reach zero ================================================

    public void TriggerPrecombat(string actionName, int sequence, bool playerInitiated) 
    {
        currentActionName = actionName;
        currentSequence = sequence;

        StartCoroutine(PrecombatCamera(playerInitiated));
        onPrecombatStart.Invoke();
    }

    // ========== CAMERA MOVEMENTS ================================================================================================

    private IEnumerator PrecombatCamera(bool playerInitiated)
    {
        elementsGUI.SetActive(false);

        if (playerInitiated)
        {
            PanCameraLeft();
        }
        else
        {
            PanCameraRight();
        }

        yield return new WaitForSeconds(waitForActionName);
        ZoomCamera();

        actorSprites.SetActive(false);
        onPrecombatEnd.Invoke();
    }

    public void ZoomCamera()
    {
        cameraAnimator.SetTrigger("Zoom");
    }

    public void PanCameraLeft()
    {
        cameraAnimator.SetTrigger("PanLeft");
    }

    public void PanCameraRight()
    {
        cameraAnimator.SetTrigger("PanRight");
    }

    // ========== REVEAL - Called by Animator ================================================================================================

    public void RevealActors() 
    {
        elementsGUI.SetActive(true);
        actorSprites.SetActive(true);
    }
}
