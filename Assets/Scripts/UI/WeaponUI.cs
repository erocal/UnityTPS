using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField] Image[] pocket;
    [SerializeField] Image[] weaponIcon;
    [SerializeField] Image[] energy;

    #endregion

    #region -- 變數參考區 --

    private ActionSystem actionSystem;

    Organism organism;

    WeaponManager weaponManager;

    #endregion

    #region -- 初始化/運作 --

    void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

        organism = Organism.Instance;

        weaponManager = organism.GetPlayer().GetComponent<WeaponManager>();
        actionSystem.OnAddWeapon += OnAddWeapon;
    }

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (weaponManager.GetWeaponAtSlotIndex(i) == null) continue;

            float value = weaponManager.GetWeaponAtSlotIndex(i).CurrentAmmoRatio;

            energy[i].fillAmount = Mathf.Lerp(energy[i].fillAmount, value, 0.2f);

            if (weaponManager.GetWeaponAtSlotIndex(i) == weaponManager.GetActiveWeapon())
            {
                pocket[i].transform.localScale = new Vector3(1f, 1f, 1f);
                pocket[i].color = Color.white;
                weaponIcon[i].color = Color.white;
                energy[i].color = Color.white;
            }
            else
            {
                // 縮小pocket
                pocket[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                pocket[i].color = Color.gray;
                weaponIcon[i].color = Color.gray;
                energy[i].color = Color.gray;
            }
        }
    }

    #endregion

    #region -- 方法參考區 --

    private void OnAddWeapon(WeaponController weapon, int index)
    {
        weaponIcon[index].enabled = true;
        weaponIcon[index].sprite = weapon.weaponIcon;
    }

    #endregion

}
