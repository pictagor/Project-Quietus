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
    public bool isDisabled;

    [SerializeField] Animator animator;
    [SerializeField] PlayerAction playerAction;
    [SerializeField] TextMeshProUGUI buttonText;

    public UnityEvent OnButtonClick;
    public UnityEvent OnButtonSelect;

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

    // ======================= ADDED TO COMBAT MENU ====================================================================
    private void OnEnable()
    {
        if (isDisabled)
        {
            animator.SetBool("Disabled", true);
            GetComponent<Selectable>().enabled = false;
            return;
        }
        else
        {
            StartCoroutine(AddToCombatMenuList());
        }
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

    // ======================= ON SELECT ====================================================================

    public void SelectThisButton()
    {
        if (isDisabled) { return; }
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData data)
    {
        animator.SetBool("Selected", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isDisabled) { return; }
        if (!CombatMenu.instance.isMenuActive) { return; }
        CombatMenu.instance.actionIndex = CombatMenu.instance.allButtons.IndexOf(this);
        animator.SetBool("Selected", true);

        if (playerAction == null)
        {
            CombatHUD.instance.HidePreviewSlider();
            Sanity.instance.DisplaySanityText();
            CombatMenu.instance.HideInfoBox();
            return;
        }

        PlayerController.instance.SetCurrentAction(playerAction);
        if (playerAction.baseSpeed == 0)
        {
            CombatHUD.instance.HidePreviewSlider();
        }
        else if (playerAction.baseSpeed > 0)
        {
            PlayerController.instance.CalculateEffectiveSpeed();
            CombatHUD.instance.ShowPreviewSlider();
        }

        if (playerAction.sanityCost > 0 || playerAction.sanityGain > 0)
        {
            Sanity.instance.DisplaySanityPreview(playerAction.sanityCost, playerAction.sanityGain);
        }
        else if (playerAction.sanityCost == 0)
        {
            Sanity.instance.DisplaySanityText();
        }

        CombatMenu.instance.DisplayInfoBox(playerAction.description);
    }
    // ======================= ON CONFIRM ====================================================================

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDisabled) { return; }
        ConfirmSelectedAction();
    }

    public void ConfirmSelectedAction()
    {
        StartCoroutine(ConfirmSelectedActionCo());
    }

    private IEnumerator ConfirmSelectedActionCo()
    {
        animator.SetTrigger("Pressed");
        yield return new WaitForSeconds(0.2f); // Wait for Button Shine

        switch (actionType)
        {
            case ActionType.Root:
                OnButtonClick.Invoke();
                break;

            default:
                PlayerController.instance.ChooseCombatAction(playerAction);
                break;
        }
    }

    // ======================= DISABLED ====================================================================

    public void DisableButton()
    {
        isDisabled = true;
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
