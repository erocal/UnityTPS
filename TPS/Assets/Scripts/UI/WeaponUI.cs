using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Image[] pocket;
    [SerializeField] Image[] weaponIcon;
    [SerializeField] Image[] energy;

    WeaponManager weaponManager;

    // Start is called before the first frame update
    void Awake()
    {
        //print("1");
        weaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        weaponManager.onAddWeapon += OnAddWeapon;
    }

    private void OnAddWeapon(WeaponController weapon, int index)
    {
        //print("3");
        weaponIcon[index].enabled = true;
        weaponIcon[index].sprite = weapon.weaponIcon;
    }

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (weaponManager.GetWeaponAtSlotIndex(i) == null) continue;

            float value = weaponManager.GetWeaponAtSlotIndex(i).currentAmmoRatio;

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
}
