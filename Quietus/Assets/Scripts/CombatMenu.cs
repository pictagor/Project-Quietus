﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CombatMenu : MonoBehaviour
{
    public bool isMenuActive;
    public bool actionQueued;
    public List<CombatButton> allButtons;
    public int actionIndex;

    [SerializeField] SpriteRenderer playerSprite;
    private Material playerMat;

    [SerializeField] RectTransform rootActions;
    [SerializeField] RectTransform attackActions;
    [SerializeField] RectTransform defendActions;
    [SerializeField] RectTransform waitActions;
    [SerializeField] TextMeshProUGUI headerText;

    [SerializeField] GameObject auraVFX;
    [SerializeField] GameObject tentacles;
    [SerializeField] GameObject rootActionGroup;

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
            DisplayRootActions();        
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    RevealRootActions();
        //}
        //else if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    HideRootActions();
        //}

        if (actionQueued) { return; }
        if (!isMenuActive) { return; }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmCurrentAction();
        }
    }


    public void ConfirmCurrentAction()
    {
        if (allButtons[actionIndex].actionType == CombatButton.ActionType.Root)
        {
            allButtons[actionIndex].ConfirmSelectedAction();
        }
        else
        {
            allButtons[actionIndex].ConfirmSelectedAction();
            HideAllMenu();
        }
    }


    public void DisplayRootActions()
    {
        if (!isMenuActive)
        {
            onMenuActive.Invoke();
        }

        attackActions.gameObject.SetActive(false);
        defendActions.gameObject.SetActive(false);
        waitActions.gameObject.SetActive(false);

        allButtons.Clear();
        rootActions.gameObject.SetActive(true);

        isMenuActive = true;

        headerText.text = null;
        headerText.transform.parent.gameObject.SetActive(false);


        rootActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        rootActions.DOLocalMoveX(0f, 0.2f);

        playerMat.SetFloat("_OutlineAlpha", 0.8f);
        auraVFX.SetActive(true);
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

        rootActions.DOLocalMoveX(-400f, 0.2f);
        attackActions.DOLocalMoveX(-400f, 0.2f);
        defendActions.DOLocalMoveX(-400f, 0.2f);
        waitActions.DOLocalMoveX(-400f, 0.2f);

        playerMat.SetFloat("_OutlineAlpha", 0f);
        auraVFX.SetActive(false);
    }

    public void DisplayATTACK()
    {
        rootActions.gameObject.SetActive(false);
        allButtons.Clear();
        attackActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "ATTACK";

        attackActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        attackActions.DOLocalMoveX(0f, 0.2f);
    }

    public void DisplayDEFEND()
    {
        rootActions.gameObject.SetActive(false);
        allButtons.Clear();
        defendActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "DEFEND";

        defendActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        defendActions.DOLocalMoveX(0f, 0.2f);
    }

    public void DisplayWAIT()
    {
        rootActions.gameObject.SetActive(false);
        allButtons.Clear();
        waitActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "REGAIN";

        waitActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        waitActions.DOLocalMoveX(0f, 0.2f);
    }


    public void RevealRootActions()
    {
        tentacles.SetActive(true);
        rootActionGroup.SetActive(true);
        playerMat.SetFloat("_OutlineAlpha", 0.8f);
    }

    public void HideRootActions()
    {
        tentacles.SetActive(false);
        rootActionGroup.SetActive(false);
        playerMat.SetFloat("_OutlineAlpha", 0f);
    }
}
