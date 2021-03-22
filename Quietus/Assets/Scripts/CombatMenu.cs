﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] TextMeshProUGUI headerText;

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

        //firstChild = rootActions.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (actionQueued) { return; }
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
            actionQueued = true;
            HideAllMenu();
        }
    }


    public void DisplayRootActions()
    {
        attackActions.gameObject.SetActive(false);
        defendActions.gameObject.SetActive(false);
        allButtons.Clear();
        rootActions.gameObject.SetActive(true);

        isMenuActive = true;

        headerText.text = null;
        headerText.transform.parent.gameObject.SetActive(false);


        rootActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        rootActions.DOLocalMoveX(0f, 0.3f);

        playerMat.SetFloat("_OutlineAlpha", 1f);
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

        rootActions.DOLocalMoveX(-400f, 0.25f);
        attackActions.DOLocalMoveX(-400f, 0.25f);
        defendActions.DOLocalMoveX(-400f, 0.25f);

        playerMat.SetFloat("_OutlineAlpha", 0f);
    }

    public void DisplayATTACK()
    {
        rootActions.gameObject.SetActive(false);
        allButtons.Clear();
        attackActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "ATTACK";

        attackActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        attackActions.DOLocalMoveX(0f, 0.25f);
    }

    public void DisplayDEFEND()
    {
        rootActions.gameObject.SetActive(false);
        allButtons.Clear();
        defendActions.gameObject.SetActive(true);

        headerText.transform.parent.gameObject.SetActive(true);
        headerText.text = "DEFEND";

        defendActions.anchoredPosition = new Vector2(-400f, rootActions.anchoredPosition.y);
        defendActions.DOLocalMoveX(0f, 0.25f);
    }
}