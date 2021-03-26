using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class CombatSprites : MonoBehaviour
{
    [SerializeField] GameObject combatCanvas;
    [SerializeField] GameObject[] combatSequence;

    public bool pauseSlider;

    public UnityEvent onCombatStarted;
    public UnityEvent onCombatFinished;

    public static CombatSprites instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CombatCamera.instance.onPrecombatStart.AddListener(PauseCombatSlider);
        CombatCamera.instance.onPrecombatEnd.AddListener(StartCombatSequence);
    }

    public void PauseCombatSlider()
    {
        pauseSlider = true;
    }

    public void ResumeCombatSlider()
    {
        pauseSlider = false;
    }

    public void StartCombatSequence()
    {
        combatSequence[CombatCamera.instance.currentSequence].SetActive(true);

        onCombatStarted.Invoke();
    }

    public void EndCombatSequence() // Called by Individual CombatSequence
    {
        ResumeCombatSlider();

        foreach (GameObject sequence in combatSequence)
        {
            sequence.SetActive(false);
        }

        onCombatFinished.Invoke();
    }
}
