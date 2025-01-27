using UnityEngine;

public class PickupLife : Pickup
{

    #region -- 資源參考區 --

    [Header("要增長的血量")]
    [SerializeField] float addHealth = 5.0f;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    #endregion

    #region -- 方法參考區 --

    protected override void PickUpItem()
    {

        Health health = player.GetComponent<Health>();
        if (health)
        {

            health.AddMaxHealth(addHealth);

            pickupRoot.SetActive(false);

        }

    }

    #endregion

}
