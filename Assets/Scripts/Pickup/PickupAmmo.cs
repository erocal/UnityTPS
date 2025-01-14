using UnityEngine;

public class PickupAmmo : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("要恢復的子彈量")]
    [SerializeField] float fullammoAmount;
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

            weaponController.Fullammo(fullammoAmount);

            pickupRoot.SetActive(false);

        }

    }

    #endregion

}
