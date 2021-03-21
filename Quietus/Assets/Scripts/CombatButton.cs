using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
//using DarkTonic.MasterAudio;

public class CombatButton : MonoBehaviour, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Animator animator;
    [SerializeField] PlayerAction playerAction;
    [SerializeField] TextMeshProUGUI buttonText;
    public UnityEvent OnButtonClick;
    public UnityEvent OnButtonSelect;
    private int index;

    //[Header("SFX")]
    //[SoundGroupAttribute] [SerializeField] string hoverSound = "Click";
    //[SerializeField] string hoverVariation = "Click 4";
    //[SoundGroupAttribute] [SerializeField] string clickSound = "Select";
    //[SerializeField] string clickVariation = "Select 4";

    void Start()
    {
        CombatMenu.instance.allButtons.Add(this);
        if (playerAction == null) { return; }
        if(!string.IsNullOrEmpty(playerAction.actionName))
        {
            buttonText.text = playerAction.actionName;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        ConfirmSelectedAction();
    }

    public void ConfirmSelectedAction()
    {
        //OnButtonClick.Invoke();
        PlayerController.instance.ChooseCombatAction(playerAction);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CombatMenu.instance.actionIndex = CombatMenu.instance.allButtons.IndexOf(this);
        animator.SetBool("Selected", true);

        if (playerAction == null) { return; }
        if (playerAction.baseSpeed > 0)
        {
            PlayerController.instance.ShowPreviewSlider(playerAction.baseSpeed);
        }
    }

    public void OnDeselect(BaseEventData data)
    {       
        animator.SetBool("Selected", false);
    }

    /// AUDIO //////////////////////////////////////////////////////

    //private void PlayHoverSound()
    //{
    //    MasterAudio.PlaySoundAndForget("Click", 1f, 3f, 0f, "Click 4");
    //}
    //
    //private void PlayClickSound()
    //{
    //    MasterAudio.PlaySoundAndForget("Click", 1f, 1f, 0f, "Click 1");
    //}
}
