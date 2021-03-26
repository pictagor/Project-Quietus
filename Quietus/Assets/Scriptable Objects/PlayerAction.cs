using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Action")]
public class PlayerAction : ScriptableObject
{

    public enum ActionType
    {
        Attack,
        Dodge,
        Block,
        Quickstep,
        Wait
    }
    public ActionType actionType;

    [Header("General")]
    public string actionName;
    public int baseSpeed;
    public int sanityCost;
    public int sequenceID;


    [Header("Attack")]
    public int baseDamage;
    public float baseHitChance;

    [Header("Wait")]
    public int sanityGain;
}
