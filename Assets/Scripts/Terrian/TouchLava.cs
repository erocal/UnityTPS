using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLava : MonoBehaviour
{
    [Header("岩漿傷害")]
    [SerializeField] float LavaDamage = 15f;

    GameObject target;
    Health targetHealth;

    bool hasbeentrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasbeentrigger) return;

        if ((other.tag == "Player" || other.tag == "Enemy"
        || other.tag == "Zombie" || other.tag == "Zombiegrounp"))
        {
            //hasbeentrigger = true;
            target = other.gameObject;

            if (target) targetHealth = target.GetComponent<Health>();

            targetHealth.TakeDamage(LavaDamage);
        }
    }
}
