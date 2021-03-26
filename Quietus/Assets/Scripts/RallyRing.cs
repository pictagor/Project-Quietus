using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RallyRing : MonoBehaviour
{
    [SerializeField] Slider playerSlider;
    [SerializeField] float minRallyDuration;
    [SerializeField] float maxRallyDuration;
    [SerializeField] float boostDuration;
    [SerializeField] float boostAmount;
    [SerializeField] float boostPct = 0.2f;
    public bool isBoosted;
    public float timer;
    [SerializeField] float maxTimer;
    [SerializeField] float minTimer;
    [SerializeField] RectTransform ring;

    [SerializeField] Image tokenImage;
    private Color originalColor;

    public bool isRallyActive;
    private Coroutine rallyCoroutine;

    public static RallyRing instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalColor = tokenImage.color;
        ring.gameObject.SetActive(false);

        CombatMenu.instance.onMenuActive.AddListener(DisableTimer);
    }


    private void Update()
    {
        if (CombatMenu.instance.isMenuActive) { return; }
        if (!CombatMenu.instance.actionQueued) { return; }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnRallyRing();
            }
        }

        if (isRallyActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ring.localScale.x <= 1.2f && ring.localScale.x > 0.6f)
                {
                    BoostActionSpeed();
                }
                else
                {
                    ring.gameObject.SetActive(false);
                    Debug.Log("Mistimed!");
                }
            }
        }
    }

    public void ChanceForRallyRing() // Called by PlayerController
    {
        timer = Random.Range(minTimer, maxTimer);
    }

    private void SpawnRallyRing()
    {
        rallyCoroutine = StartCoroutine(SpawnRallyRingCo());
    }

    private IEnumerator SpawnRallyRingCo()
    {
        if (isRallyActive) { yield return null; }
        isRallyActive = true;
        ring.gameObject.SetActive(true);
        ring.localScale = new Vector2(3f, 3f);

        float rallyDuration = Random.Range(minRallyDuration, maxRallyDuration);
        ring.DOScale(0.5f, rallyDuration);
        yield return new WaitForSeconds(rallyDuration);
        isRallyActive = false;
    }

    private void BoostActionSpeed()
    {
        if (isBoosted) { return; }

        StartCoroutine(BoostActionSpeedCo());
    }

    private IEnumerator BoostActionSpeedCo()
    {
        isBoosted = true;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tokenImage.DOColor(Color.white, 0.2f));
        mySequence.Append(tokenImage.DOColor(originalColor, 0.2f));

        boostAmount = PlayerController.instance.currentAction.baseSpeed * boostPct;
        float newValue = Mathf.Clamp(PlayerController.instance.combatSlider.value - boostAmount, 0, 100);
        DOTween.To(() => PlayerController.instance.combatSlider.value, x => PlayerController.instance.combatSlider.value = x, newValue, boostDuration);
        yield return new WaitForSeconds(boostDuration);


        ring.gameObject.SetActive(false);
        isBoosted = false;
    }

    private void OnDisable()
    {
        //StopAllCoroutines();
        isBoosted = false;
    }

    private void DisableTimer()
    {
        timer = -1f;
    }
}
