using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenu : MonoBehaviour
{
    public bool isMenuActive;
    public bool actionQueued;
    public List<CombatButton> allButtons;
    public int actionIndex;

    [SerializeField] Animator animator;
    public static CombatMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (!actionQueued)
        {
            DisplayMenu();
        }
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
        allButtons[actionIndex].ConfirmSelectedAction();
        actionQueued = true;
        HideMenu();
    }


    public void DisplayMenu()
    {
        isMenuActive = true;
        animator.SetBool("Display", true);
    }

    public void HideMenu()
    {
        isMenuActive = false;
        animator.SetBool("Display", false);
    }
}
