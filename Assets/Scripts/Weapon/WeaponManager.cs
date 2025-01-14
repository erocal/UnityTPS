using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("一開始就有的武器")]
    [SerializeField] List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("儲存武器位置的Parent，武器會被加在這裡")]
    [SerializeField] Transform equipWeaponParent;

    [Header("瞄準的準備時間")]
    [SerializeField] float aimTime = 2f;

    #region -- 變數參考區 --

    private ActionSystem actionSystem;

    // 目前裝備的武器清單位置
    int activeWeaponIndex;

    // 武器最多三個
    WeaponController[] weapons = new WeaponController[3];
    PlayerController player;
    InputController input;

    bool isAim;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

    }

    void Start()
    {
        // 初始狀態
        activeWeaponIndex = -1;

        input = GameManagerSingleton.Instance.InputController;
        player = GetComponent<PlayerController>();
        actionSystem.OnAim += OnAim;

        foreach (WeaponController weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwichWeapon(1);

    }

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

    #endregion

    /// <summary>
    /// 切換武器位置到(現在武器位置+傳入的參數)的位置
    /// </summary>
    /// <param name="addIndex">向左或向右的參數</param>
    public void SwichWeapon(int addIndex)
    {

        int newWeaponIndex;
        // 0 1 2
        if (activeWeaponIndex + addIndex > weapons.Length - 1)
        {
            newWeaponIndex = 0; //從最右切換到最左
        }
        else if (activeWeaponIndex + addIndex < 0)
        {
            newWeaponIndex = weapons.Length - 1; //從最左切換到最右
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;//中間切換左右
        }

        // 換武器到NextIndex
        SwichToWeaponIndex(newWeaponIndex);

    }

    /// <summary>
    /// 顯示對應武器位置的武器
    /// </summary>
    /// <param name="index">要顯示武器的對應武器位置</param>
    private void SwichToWeaponIndex(int index)
    {

        // 確保傳入的參數在武器清單的範圍內
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

    /// <summary>
    /// 回傳當前的武器，沒有就null
    /// </summary>
    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    /// <summary>
    /// 根據武器槽的位置回傳對應的武器
    /// </summary>
    /// <param name="index">武器槽的位置</param>
    public WeaponController GetWeaponAtSlotIndex(int index)
    {

        // 找到weapon在武器槽位置(index)並回傳該武器
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

    /// <summary>
    /// 新增武器到武器槽
    /// </summary>
    /// <param name="weaponPrefab">傳入武器</param>
    /// <returns>是否成功新增武器</returns>
    public bool AddWeapon(WeaponController weaponPrefab)
    {

        if (HasWeapon(weaponPrefab))
        {
            return false;
        }

        // 找到下一個沒有裝武器的武器槽
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                // 產生Weapon到設定好的位置底下
                WeaponController weaponInstance = Instantiate(weaponPrefab, equipWeaponParent);

                weaponInstance.SourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);

                weapons[i] = weaponInstance;

                actionSystem.AddWeapon(weaponInstance, i);

                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// 確認玩家是否已經擁有傳入的武器
    /// </summary>
    /// <param name="weaponPrefab">傳入的武器</param>
    private bool HasWeapon(WeaponController weaponPrefab)
    {

        foreach (WeaponController weapon in weapons)
        {
            if (weapon != null && weapon.SourcePrefab == weaponPrefab.gameObject)
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
