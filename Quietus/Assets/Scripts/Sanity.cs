using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sanity : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sanityText;
    public int sanityCounter;
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] int doomThreshold = 100;

    public static Sanity instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplaySanityText();
        PlayerController.instance.onActionChosen.AddListener(IncreaseSanity);
        CombatMenu.instance.onMenuActive.AddListener(CheckDOOM);
    }

    public void DisplaySanityPreview(int cost, int gain) // Called by Combat Button
    {
        int previewCounter = Mathf.Clamp(sanityCounter + cost - gain, 0, 999);
        sanityText.text = previewCounter.ToString();
        sanityText.color = color2;
    }
    
    public void IncreaseSanity()
    {
        sanityCounter += PlayerController.instance.currentAction.sanityCost;
        DisplaySanityText();
    }

    private void CheckDOOM()
    {
        if (sanityCounter >= doomThreshold)
        {
            StatusEffect.instance.InflictDOOM();
        }
        else
        {
            StatusEffect.instance.RemoveDOOM();
        }
    }

    public void RegainSanity()
    {
        sanityCounter = Mathf.Clamp(sanityCounter - PlayerController.instance.currentAction.sanityGain, 0, 999);
        DisplaySanityText();
    }

    public void DisplaySanityText()
    {
        sanityText.text = sanityCounter.ToString();
        sanityText.color = color1;
    }
}
