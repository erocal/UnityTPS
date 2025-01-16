using System;
using UnityEngine;
using System.Threading.Tasks;

public sealed class ActionSystem
{

    #region -- Action --

    public event Action<bool> OnAim;
    public event Action OnCaplock;
    public event Action OnCaplockUp;

    public event Action<WeaponController, int> OnAddWeapon;

    // 當受到攻擊時觸發的委派事件
    public event Action<int> OnDamage;
    // 當人物死亡時觸發的委派事件
    public event Action<int> OnDie;

    public event Func<int, Task> OnMapAreaSwitch;

    public event Action<Vector3, MapAreaType> OnSpawnPointUpdate;

    #endregion

    #region -- 方法參考區 --

    public void Aim(bool isAim)
    {
        OnAim?.Invoke(isAim);
    }

    public void PlayerSpeedUp()
    {
        OnCaplock?.Invoke();
    }

    public void ReleaseCaplock()
    {
        OnCaplockUp?.Invoke();
    }

    public void AddWeapon(WeaponController weaponController, int index)
    {
        OnAddWeapon?.Invoke(weaponController, index);
    }

    public void Damage(int id)
    {
        OnDamage?.Invoke(id);
    }

    public void Death(int id)
    {
        OnDie?.Invoke(id);
    }

    public async Task MapAreaSwitch(int mapAreaTriggerid)
    {
        await OnMapAreaSwitch?.Invoke(mapAreaTriggerid);
    }

    public void SpawnPointUpdate(Vector3 spawnPos, MapAreaType mapArea)
    {

        OnSpawnPointUpdate?.Invoke(spawnPos, mapArea);

    }

    #endregion

}