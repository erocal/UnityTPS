using UnityEngine;

public class SpawnPoint : Pickup
{

    #region -- 資源參考區 --

    [Header("地圖區域"), Tooltip("這個重生點所屬的地圖區域")]
    [SerializeField] MapAreaType mapArea;

    [Header("重生點特效"), Tooltip("觸發重生點時提示玩家的特效")]
    [SerializeField] GameObject spawnPointParticle;

    #endregion

    #region -- 變數參考區 --

    private ActionSystem actionSystem;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

    }

    #endregion

    #region -- 方法參考區 --

    protected override void PickUpItem()
    {

        actionSystem.SpawnPointUpdate(this.transform.position, mapArea);

        // 觸發特效
        var spawnPointParticle = Instantiate(this.spawnPointParticle, this.transform);
        spawnPointParticle.SetActive(true);

    }

    #endregion

}
