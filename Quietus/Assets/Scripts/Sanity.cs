using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sanity : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sanityText;
    [SerializeField] int sanityCounter;
    [SerializeField] Color color1;
    [SerializeField] Color color2;

    public static Sanity instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplaySanityText();
        PlayerController.instance.onActionChosen.AddListener(IncreaseSanity);
    }

    public void DisplaySanityPreview(int cost) // Called by Combat Button
    {
        sanityText.text = (sanityCounter + cost).ToString();
        sanityText.color = color2;
    }
    
    public void IncreaseSanity()
    {
        sanityCounter += PlayerController.instance.currentAction.sanityCost;
        DisplaySanityText();
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
