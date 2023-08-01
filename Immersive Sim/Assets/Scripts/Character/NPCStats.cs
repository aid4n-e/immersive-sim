using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    private DamageCalculator damage;

    public float health;
    public float armour;

    public GameObject[] limbs;
    public int hitLimb;

    private bool isDead;

    void Start()
    {
        damage = new DamageCalculator();
    }

    void Update()
    {
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void InflictDamage(float dam, float pen)
    {
        float finalDam = damage.Damage(dam, pen, armour);
        health -= finalDam;

        if(health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        if(true)
        {
            isDead = true;
            Destroy(limbs[hitLimb-1]);
        }

        // ragdoll
    }

}



public class DamageCalculator
{
    public float Damage(float dam, float pen, float arm)  // Requires damage, penetration, and victim armour values
    {
        return (dam / (Mathf.Clamp(arm - pen, 1, 10)));  // final = damage / (armor - penetration)
    }
}



