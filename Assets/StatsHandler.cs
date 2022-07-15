using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    public Weapon MainWeapon;

    public int Strength;
    public int Dexterity;
    public int Constitution;

    public int MaxHeath;
    public int CurrentHealth;


    public System.Action deathCall;
    public System.Action<int> DamageCall;

    
    private void Start()
    {
        CurrentHealth = MaxHeath;
    }

    public int getIncomingDamageReduciton()
    {
        return 0;
    }

    public int getOutgoingDamageIncrease()
    {
        return Strength / 2;
    }

    public void HandleDamageTaken(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            handleDeath();
        }
        else
        {
            DamageCall.Invoke(damage);
        }
    }

    void handleDeath()
    {
        deathCall.Invoke();
    }
}
