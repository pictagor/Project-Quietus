using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [Header("Speed Boost")]
    public float speedModifier;

    public static StatusEffect instance;

    private void Awake()
    {
        instance = this;
    }
}
