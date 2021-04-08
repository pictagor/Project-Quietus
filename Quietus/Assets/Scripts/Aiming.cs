using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class Aiming : MonoBehaviour
{
    [Header("Aiming")]
    public bool aimActive;
    [SerializeField] RectTransform origin;
    [SerializeField] RectTransform topLine;
    [SerializeField] RectTransform bottomLine;
    [SerializeField] float aimSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Image iconImage;
    private Image topLineImage;
    private Image bottomLineImage;
    private Color originalColor_top;
    private Color originalColor_icon;
    private Vector3 originalAngle_origin;
    private Vector3 originalAngle_top;
    private Vector3 originalAngle_bottom;
    private bool isAiming;
    private bool flashing;

    Sequence aimSequence;

    public UnityEvent onAimingFinished;

    [Header("Weakspot")]
    [SerializeField] Image spotImage;
    [SerializeField] Color spotColor_1;
    [SerializeField] Color spotColor_2;

    public static Aiming instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        topLineImage = topLine.GetComponent<Image>();
        bottomLineImage = bottomLine.GetComponent<Image>();

        originalColor_top = topLineImage.color;
        originalColor_icon = iconImage.color;

        originalAngle_origin = origin.eulerAngles;
        originalAngle_top = topLine.eulerAngles;
        originalAngle_bottom = bottomLine.eulerAngles;

        FlashWeakSpot();
    }

    public void TriggerAim()
    {
        aimActive = true;

        origin.gameObject.SetActive(true);
        topLine.gameObject.SetActive(true);
        bottomLine.gameObject.SetActive(true);
        iconImage.gameObject.SetActive(true);
        spotImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 forward = topLine.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(origin.position, forward, Color.green);

        if (!aimActive) { return; }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateOriginDown();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            RotateOriginUp();
        }

        // Aim
        if (Input.GetKey(KeyCode.Q))
        {
            AimingReticule();
        }

        if(Input.GetKeyUp(KeyCode.Q))
        {
            FinishAim();
        }
    }

    private void FinishAim()
    {
        isAiming = false;
        aimSequence.Kill();
        topLineImage.color = originalColor_top;

        onAimingFinished.Invoke();

        DetectHit();
    }

    private void DetectHit()
    {
        float angleZ = Random.Range(topLine.eulerAngles.z, bottomLine.eulerAngles.z);
        Debug.Log(angleZ);
        topLine.eulerAngles = bottomLine.eulerAngles = new Vector3(topLine.eulerAngles.x, topLine.eulerAngles.y, angleZ);

        RaycastHit hit;
        if (Physics.Raycast(origin.position, topLine.transform.TransformDirection(Vector3.down), out hit, 100f, layerMask))
        {
            PlayerController.instance.currentAction.STUN = true;
        }
        else
        {
            PlayerController.instance.currentAction.STUN = false;
        }

        spotImage.gameObject.SetActive(false);
        iconImage.gameObject.SetActive(false);
        topLine.gameObject.SetActive(false);
        bottomLine.gameObject.SetActive(false);

        PlayerController.instance.PerformAction();
    }

    private void RotateOriginUp()
    {
        origin.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * rotateSpeed);
    }

    private void RotateOriginDown()
    {
        origin.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * rotateSpeed);
    }

    private void AimingReticule()
    {
        isAiming = true;
        topLine.gameObject.SetActive(true);
        bottomLine.gameObject.SetActive(true);

        if (Quaternion.Angle(topLine.rotation, bottomLine.rotation) < 1 || topLine.eulerAngles.z < bottomLine.eulerAngles.z)
        {
            topLine.eulerAngles = new Vector3(topLine.eulerAngles.x, topLine.eulerAngles.y, bottomLine.eulerAngles.z);
            FlashAimLine();
            return;
        }
        topLine.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * aimSpeed);
        bottomLine.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * aimSpeed);
    }

    private void FlashAimLine()
    {
        if (flashing) { return; }
        flashing = true;

        topLineImage.color = Color.white;
        iconImage.color = Color.white;

        aimSequence = DOTween.Sequence();
        aimSequence
            .Append(topLineImage.DOFade(0.2f, 0.1f))
            .Append(topLineImage.DOFade(1f, 0.1f))
            .SetLoops(-1);
    }


    private void FlashWeakSpot()
    {
        Sequence spotSequence = DOTween.Sequence();
        spotSequence
            .Append(spotImage.DOColor(spotColor_1, 0.3f))
            .Append(spotImage.DOColor(spotColor_2, 0.3f))
            .SetLoops(-1);
    }

    private void ResetAim()
    {
        flashing = false;
        aimSequence.Kill();

        topLineImage.color = originalColor_top;
        iconImage.color = originalColor_icon;

        origin.eulerAngles = originalAngle_origin;
        topLine.eulerAngles = originalAngle_top;
        bottomLine.eulerAngles = originalAngle_bottom;

        iconImage.gameObject.SetActive(false);
        topLine.gameObject.SetActive(false);
        bottomLine.gameObject.SetActive(false);
        spotImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ResetAim();
    }


}
