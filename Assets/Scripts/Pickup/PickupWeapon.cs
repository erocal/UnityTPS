using UnityEngine;

public class PickupWeapon : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("撿起來會得到的武器")]
    [SerializeField] WeaponController weaponPrefab;
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
