using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Action")]
public class EnemyAction : ScriptableObject
{
    [Header("Attributes")]
    public string actionName;
    public int baseDamage;
    public int minSpeed;
    public int maxSpeed;
    public int sequenceID;
    public float baseHitChance;
}
