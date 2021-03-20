using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//using DarkTonic.MasterAudio;

public class CombatButton : MonoBehaviour, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Animator animator;
    public UnityEvent OnButtonClick;
    public UnityEvent OnButtonSelect;

    //[Header("SFX")]
    //[SoundGroupAttribute] [SerializeField] string hoverSound = "Click";
    //[SerializeField] string hoverVariation = "Click 4";
    //[SoundGroupAttribute] [SerializeField] string clickSound = "Select";
    //[SerializeField] string clickVariation = "Select 4";

    void Awake()
    {
 
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonClick.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        
        animator.SetBool("Selected", true);
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
