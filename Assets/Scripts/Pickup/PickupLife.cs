using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLife : MonoBehaviour
{
    [Header("要增長的血量")]
    [SerializeField] float addHealth = 5.0f;
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
        if (health)
        {
            health.AddMaxHealth(addHealth);

            //Destroy(pickupRoot);
            pickupRoot.SetActive(false);
        }
    }
}
