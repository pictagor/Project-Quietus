using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
//using DarkTonic.MasterAudio;

public class CombatButton : MonoBehaviour, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    public enum ActionType
    {
        Root,
        Attack,
        Defend,
        Stance,
        Wait
    }

    public ActionType actionType;

    public bool rootAction;
    public bool firstSelected;

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
        if (playerAction == null) { return; }
        if(!string.IsNullOrEmpty(playerAction.actionName))
        {
            buttonText.text = playerAction.actionName;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AddToCombatMenuList());
    }

    private IEnumerator AddToCombatMenuList()
    {
        yield return new WaitForSeconds(0.1f); // Wait 0.1f for CombatMenu instance = this
        CombatMenu.instance.allButtons.Add(this);
        if (this.transform.parent.GetChild(0).gameObject == this.gameObject)
        {
            SelectThisButton();
        }
    }

    public void SelectThisButton()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ConfirmSelectedAction();
    }

    public void ConfirmSelectedAction()
    {
        StartCoroutine(ConfirmSelectedActionCo());
    }

    private IEnumerator ConfirmSelectedActionCo()
    {
        animator.SetTrigger("Pressed");
        yield return new WaitForSeconds(0.2f);

        switch (actionType)
        {
            case ActionType.Root:
                OnButtonClick.Invoke();
                break;

            case ActionType.Defend:
                PlayerController.instance.ChooseCombatAction(playerAction);
                break;

            case ActionType.Attack:
                PlayerController.instance.ChooseCombatAction(playerAction);
                break;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (CombatMenu.instance.actionQueued) { return; }
        CombatMenu.instance.actionIndex = CombatMenu.instance.allButtons.IndexOf(this);
        animator.SetBool("Selected", true);

        if (playerAction == null || playerAction.baseSpeed == 0)
        {
            PlayerController.instance.HidePreviewSlider();
        }
        else if (playerAction.baseSpeed > 0)
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
