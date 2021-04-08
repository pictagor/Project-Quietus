using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatMenu : MonoBehaviour
{
    public enum MenuState
    {
        ROOT,
        ATTACK,
        DEFEND,
        WAIT
    }

    public MenuState menuState;

    public bool isMenuActive;
    public bool actionQueued;
    public List<CombatButton> allButtons;
    public int actionIndex;

    [SerializeField] SpriteRenderer playerSprite;
    private Material playerMat;

    [Header("Action Groups")]
    [SerializeField] GameObject rootActionGroup;
    [SerializeField] RectTransform attackActions;
    [SerializeField] RectTransform defendActions;
    [SerializeField] RectTransform waitActions;
    [SerializeField] TextMeshProUGUI headerText;

    [Header("Other GUI Elements")]
    [SerializeField] GameObject auraVFX;
    [SerializeField] GameObject tentacles;

    [Header("Description Box")]
    [SerializeField] GameObject infoBox;
    [SerializeField] TextMeshProUGUI descriptionText;

    public UnityEvent onMenuActive;

    public static CombatMenu instance;

    private void Awake()
    {
        instance = this;
        playerMat = playerSprite.material;
    }

    private void OnEnable()
    {
        if (!actionQueued)
        {
            //DisplayRootActions();
            RevealRootActions();
        }
    }

    private void Update()
    {
        if (actionQueued) { return; }
        if (!isMenuActive) { return; }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmCurrentAction();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            RevealATTACK();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            RevealDEFEND();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            RevealWAIT();
        }
    }


    public void ConfirmCurrentAction()
    {
        if (allButtons[actionIndex] == null) { return; }
        allButtons[actionIndex].ConfirmSelectedAction();
        if (allButtons[actionIndex].actionType != CombatButton.ActionType.Root)
        {
            HideAllMenu();
        }
    }

    public void HideAllMenu()
    {
        StartCoroutine(HideAllMenuCo());
    }

    private IEnumerator HideAllMenuCo()
    {
        yield return new WaitForSeconds(0.2f); // Wait for Button Animation
        isMenuActive = false;
        allButtons.Clear();

        headerText.text = null;
        headerText.transform.parent.gameObject.SetActive(false);

        //rootActions.DOLocalMoveX(-400f, 0.2f);
        attackActions.DOLocalMoveX(-400f, 0.2f);
        defendActions.DOLocalMoveX(-400f, 0.2f);
        waitActions.DOLocalMoveX(-400f, 0.2f);

        playerMat.SetFloat("_OutlineAlpha", 0f);
        auraVFX.SetActive(false);

        HideInfoBox();
    }

    public void RevealRootActions()
    {
        if (!isMenuActive)
        {
            onMenuActive.Invoke();
        }

        menuState = MenuState.ROOT;

        attackActions.gameObject.SetActive(false);
        defendActions.gameObject.SetActive(false);
        waitActions.gameObject.SetActive(false);

        tentacles.SetActive(true);
        rootActionGroup.SetActive(true);

        playerMat.SetFloat("_OutlineAlpha", 0.8f);
        auraVFX.SetActive(true);

        isMenuActive = true;

        headerText.text = null;
        headerText.transform.parent.gameObject.SetActive(false);

        HideInfoBox();
    }

    public void HideRootActions()
    {
        tentacles.SetActive(false);
        rootActionGroup.SetActive(false);
        playerMat.SetFloat("_OutlineAlpha", 0f);
    }

    public void RevealATTACK()
    {
        if (menuState == MenuState.ATTACK) { return; }
        menuState = MenuState.ATTACK;

        rootActionGroup.gameObject.SetActive(false);
        tentacles.SetActive(false);

        allButtons.Clear();
        attackActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "ATTACK";

        attackActions.DOLocalMoveX(0f, 0.2f);
    }

    public void RevealDEFEND()
    {
        if (menuState == MenuState.DEFEND) { return; }
        menuState = MenuState.DEFEND;

        rootActionGroup.gameObject.SetActive(false);
        tentacles.SetActive(false);

        allButtons.Clear();
        defendActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "DEFEND";

        defendActions.DOLocalMoveX(0f, 0.2f);
    }

    public void RevealWAIT()
    {
        if (menuState == MenuState.WAIT) { return; }
        menuState = MenuState.WAIT;

        rootActionGroup.gameObject.SetActive(false);
        tentacles.SetActive(false);

        allButtons.Clear();
        waitActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "REGAIN";

        waitActions.DOLocalMoveX(0f, 0.2f);
    }

    public void DisplayInfoBox(string description)
    {
        infoBox.SetActive(true);
        descriptionText.text = description;
    }

    public void HideInfoBox()
    {
        infoBox.SetActive(false);
        descriptionText.text = null;
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
