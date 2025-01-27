using UnityEngine;

public class PickupHealth : Pickup
{

    #region -- 資源參考區 --

    [Header("要恢復的血量")]
    [SerializeField] float healAmount = 30f;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    #endregion

    #region -- 方法參考區 --

    protected override void PickUpItem()
    {

        Health health = player.GetComponent<Health>();
        if (health && health.maxHealth != health.currentHealth)
        {

            health.Heal(healAmount);

            pickupRoot.SetActive(false);

        }

    }

    #endregion

}
