using UnityEngine;

public class PickupAmmo : Pickup
{

    #region -- 資源參考區 --

    [Header("要恢復的子彈量")]
    [SerializeField] float fullammoAmount;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 撿起彈藥
    /// </summary>
    protected override void PickUpItem()
    {

        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
        WeaponController weaponController = weapon.GetComponent<WeaponController>();
        if (weaponController)
        {

            weaponController.Fullammo(fullammoAmount);

            pickupRoot.SetActive(false);

        }

    }

    #endregion

}
