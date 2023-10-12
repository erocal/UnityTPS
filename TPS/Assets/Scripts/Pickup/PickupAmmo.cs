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

    /// <summary>
    /// 撿起彈藥
    /// </summary>
    /// <param name="player">玩家</param>
    void OnPick(GameObject player)
    {
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        WeaponController weaponController = weapon.GetComponent<WeaponController>();
        if (weaponController)
        {
            weaponController.fullammo(fullammoAmount);

            //Destroy(pickupRoot);
            pickupRoot.SetActive(false);
        }
    }
}
