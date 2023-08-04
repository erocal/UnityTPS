using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [Header("一開始就有的武器")]
    [SerializeField] List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("儲存武器位置的Parent，武器會被加在這裡")]
    [SerializeField] Transform equipWeaponParent;

    [Header("瞄準的準備時間")]
    [SerializeField] float aimTime = 2f;

    public event Action<WeaponController, int> onAddWeapon;

    // 目前裝備的武器清單位置
    int activeWeaponIndex;

    // 武器最多三個
    WeaponController[] weapons = new WeaponController[3];
    PlayerController player;
    InputController input;

    bool isAim;

    // Start is called before the first frame update
    void Start()
    {
        // 初始狀態
        activeWeaponIndex = -1;

        input = GameManagerSingleton.Instance.InputController;
        player = GetComponent<PlayerController>();
        player.onAim += OnAim;

        foreach (WeaponController weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwichWeapon(1);
    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        if (activeWeapon && isAim)
        {
            // 處理射擊
            activeWeapon.HandleShootInput(input.GetFireInputDown(), input.GetFireInputHeld(), input.GetFireInputUp());
        }

        int swichWeaponInput = input.GetSwichWeaponInput();
        if (swichWeaponInput != 0)
        {
            SwichWeapon(swichWeaponInput);
        }
    }

    // 切換武器
    public void SwichWeapon(int addIndex)
    {
        int newWeaponIndex = -1;
        // 0 1 2
        if (activeWeaponIndex + addIndex > weapons.Length - 1)
        {
            newWeaponIndex = 0;
        }
        else if (activeWeaponIndex + addIndex < 0)
        {
            newWeaponIndex = weapons.Length - 1;
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;
        }
        //print("weapons.Length" + weapons.Length);
        //print("activeWeaponIndex : " + activeWeaponIndex);
        //print("newWeaponIndex : " + newWeaponIndex);
        // 換武器到NextIndex
        SwichToWeaponIndex(newWeaponIndex);
    }

    private void SwichToWeaponIndex(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            if (GetWeaponAtSlotIndex(index) != null)
            {
                // 如果目前已裝備武器，就隱藏
                if (GetActiveWeapon() != null)
                {
                    GetActiveWeapon().ShowWeapon(false);
                }

                // 顯示武器
                activeWeaponIndex = index;
                GetActiveWeapon().ShowWeapon(true);
            }
        }
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // 找到weapon在slot的位置並回傳該武器
        if (index >= 0 && index < weapons.Length && weapons[index] != null)
        {
            return weapons[index];
        }
        else if (index >= weapons.Length)
        {
            index = 0;
            return weapons[index];
        }

        // 如果沒有找到武器
        return null;
    }

    public bool AddWeapon(WeaponController weaponPrefab)
    {
        // 確認目前Slot裡沒有武器
        if (HasWeapon(weaponPrefab))
        {
            return false;
        }

        // 找到下一個空間沒有裝武器
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                // 產生Weapon到設定好的位置底下
                WeaponController weaponInstance = Instantiate(weaponPrefab, equipWeaponParent);

                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);

                weapons[i] = weaponInstance;

                //print("2");
                onAddWeapon?.Invoke(weaponInstance, i);

                return true;
            }
        }
        return false;
    }

    private bool HasWeapon(WeaponController weaponPrefab)
    {
        foreach (WeaponController weapon in weapons)
        {
            if (weapon != null && weapon.sourcePrefab == weaponPrefab.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void OnAim(bool value)
    {
        if (value)
        {
            StopAllCoroutines();
            StartCoroutine(DelayAim());
        }
        else
        {
            isAim = value;
        }
    }

    IEnumerator DelayAim()
    {
        yield return new WaitForSecondsRealtime(aimTime);
        isAim = true;
    }
}
