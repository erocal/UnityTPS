using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [Header("要恢復的血量")]
    [SerializeField] float healAmount = 30f;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.OnPick += OnPick;
    }

    void OnPick(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (health && health.maxHealth != health.currentHealth)
        {
            health.Heal(healAmount);

           //Destroy(pickupRoot);
            pickupRoot.SetActive(false);
        }
    }
}
