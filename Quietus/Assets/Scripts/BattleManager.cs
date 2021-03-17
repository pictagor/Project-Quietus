using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject elementsGUI;
    [SerializeField] Animator cameraAnimator;
    [SerializeField] GameObject actorSprites;
    [SerializeField] GameObject combatSprites;
    [SerializeField] CameraFilterPack_Blur_Focus cameraFocus;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TriggerCombat();
        }
    }

    public void TriggerCombat()
    {
        elementsGUI.SetActive(false);
        actorSprites.SetActive(false);
        combatSprites.SetActive(true);

        cameraAnimator.SetTrigger("Zoom");
    }

    public void RevealGUI() // Called by Animator
    {

        combatSprites.SetActive(false);
    }

    public void RevealActors() // Called by Animator
    {
        elementsGUI.SetActive(true);
        actorSprites.SetActive(true);
    }
}
