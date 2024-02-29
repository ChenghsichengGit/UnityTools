using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damage : MonoBehaviour
{
    private enum DamageType { InstantDamage, ContinuousDamage }
    private enum TriggerType { normal }

    [SerializeField] private DamageType damageType;
    [SerializeField] private float damage;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Health target = other.GetComponent<Health>();

        if (target)
        {
            target.ModifyHealth(-damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

}
