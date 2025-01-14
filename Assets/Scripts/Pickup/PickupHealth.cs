using UnityEngine;

public class PickupHealth : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("要恢復的血量")]
    [SerializeField] float healAmount = 30f;
    [Header("要隱藏的gameobject根節點")]
    [SerializeField] GameObject pickupRoot;

    #endregion

    #region -- 變數參考區 --

    Pickup pickup;

    #endregion

    #region -- 初始化/運作 --

    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.OnPick += OnPick;
    }

    #endregion

    #region -- 方法參考區 --

    void OnPick(GameObject player)
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
