using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Sword")]
public class Weapon : ScriptableObject
{
    public int AttackDamage;
    public float AttackTime;
    public float AttackRange;

    public float AttackLeftArcLimit;
    public float AttackRightArcLimit;

    public Sprite WeaponTexture;

    public enum AttackType
    {
        Slash
    }
    public AttackType WeaponAttackType = AttackType.Slash;

}
