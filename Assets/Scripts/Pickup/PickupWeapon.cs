using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [Header("撿起來會得到的武器")]
    [SerializeField] WeaponController weaponPrefab;
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
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        if(weaponManager)
        {
            if(weaponManager.AddWeapon(weaponPrefab))
            {
                // 換武器
                if(weaponManager.GetActiveWeapon() == null)
                {
                    weaponManager.SwichWeapon(1);
                }

                //Destroy(pickupRoot);
                pickupRoot.SetActive(false);
            }
        }
    }
}
