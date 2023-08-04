using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    [Header("要恢復的子彈量")]
    [SerializeField] float fullammoAmount;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    void OnPick(GameObject player)
    {
        player = GameObject.FindGameObjectWithTag("Weapon");
        WeaponController weaponController = player.GetComponent<WeaponController>();
        if (weaponController)
        {
            weaponController.fullammo(fullammoAmount);

            //Destroy(pickupRoot);
            pickupRoot.SetActive(false);
        }
    }
}
