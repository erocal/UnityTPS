using UnityEngine;

public class PickupWeapon : Pickup
{

    #region -- 資源參考區 --

    [Header("撿起來會得到的武器")]
    [SerializeField] WeaponController weaponPrefab;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    #endregion

    #region -- 方法參考區 --

    protected override void PickUpItem()
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

                pickupRoot.SetActive(false);

            }
        }

    }

    #endregion

}
