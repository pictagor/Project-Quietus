using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Action")]
public class PlayerAction : ScriptableObject
{
    [Header("General")]
    public string actionName;
    public int baseSpeed;
    public int sequenceID;

    [Header("Attack")]
    public int baseDamage;
    public float baseHitChance;

    [Header("Defend")]
    public float enemyHitPenalty;
}
