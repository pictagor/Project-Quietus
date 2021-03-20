using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class ActionName : MonoBehaviour
{
    [SerializeField] Image smudge;
    private Material smudgeMat;
    [SerializeField] TextMeshProUGUI nameText;

    // Start is called before the first frame update
    private void Awake()
    {
        smudgeMat = smudge.material;
    }

    private void OnEnable()
    {
        RevealActionName();
    }

    private void RevealActionName()
    {
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 0);
        smudgeMat.SetFloat("_FadeAmount", 0.4f);

        smudgeMat.DOFloat(0f, "_FadeAmount", 0.5f);
        nameText.DOFade(1f, 0.5f);
        //Sequence mySequence = DOTween.Sequence();
        //mySequence.Append(smudgeMat.DOFloat(0f, "_FadeAmount", 0.25f));
        //mySequence.Append(nameText.DOFade(1f, 0.25f));
    }

}
