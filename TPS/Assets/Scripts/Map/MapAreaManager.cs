using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum MapArea
{
    StartArea,
    PlainArea,
    PlagueDoctorArea,
    MutantArea
}

public class MapAreaManager : MonoBehaviour
{

    [Header("敵人"), Tooltip("地圖上預先生成的全部敵人")]
    [SerializeField] List<GameObject> fullEnemyList = new List<GameObject>();
    [Header("區域敵人")]
    [Tooltip("StartArea區域預先生成的敵人")]
    [SerializeField] List<GameObject> StartAreaEnemyList = new List<GameObject>();
    [Tooltip("PlainArea區域預先生成的敵人")]
    [SerializeField] List<GameObject> PlainAreaEnemyList = new List<GameObject>();
    [Tooltip("PlagueDoctorArea區域預先生成的敵人")]
    [SerializeField] List<GameObject> PlagueDoctorAreaEnemyList = new List<GameObject>();
    [Tooltip("MutantArea區域預先生成的敵人")]
    [SerializeField] List<GameObject> MutantAreaEnemyList = new List<GameObject>();

    #region -- 參數參考區 --

    private Dictionary<int, GameObject> mapAreaDictionary = new Dictionary<int, GameObject>()
    {
        { 0, null},
        { 1, null},
        { 2, null},
        { 3, null},
        { 4, null},
    };

    private Dictionary<int, List<GameObject>> areaEnemyDictionary = new Dictionary<int, List<GameObject>>();

    #endregion

    #region -- 初始/運作 --

    private void Awake()
    {
        areaEnemyDictionary.Add((int)MapArea.StartArea, StartAreaEnemyList);
        areaEnemyDictionary.Add((int)MapArea.PlainArea, PlainAreaEnemyList);
        areaEnemyDictionary.Add((int)MapArea.PlagueDoctorArea, PlagueDoctorAreaEnemyList);
        areaEnemyDictionary.Add((int)MapArea.MutantArea, MutantAreaEnemyList);
    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 切換地圖區域
    /// </summary>
    /// <param name="mapAreaTriggerid">區域id</param>
    /// /// <param name="enemyList">區域存在敵人</param>
    public async Task SwitchMapArea(int mapAreaTriggerid)
    {

        switch (mapAreaTriggerid)
        {
            case 0:
                await LoadMapAreaAsync((int)MapArea.StartArea, MapArea.StartArea);
                await LoadMapAreaAsync((int)MapArea.PlainArea, MapArea.PlainArea);
                UnloadMapArea(MapArea.PlagueDoctorArea);
                UnloadMapArea(MapArea.MutantArea);
                break;
            case 1:
            case 2:
                UnloadMapArea(MapArea.StartArea);
                await LoadMapAreaAsync((int)MapArea.PlainArea, MapArea.PlainArea);
                await LoadMapAreaAsync((int)MapArea.PlagueDoctorArea, MapArea.PlagueDoctorArea);
                UnloadMapArea(MapArea.MutantArea);
                break;
            case 3:
                UnloadMapArea(MapArea.StartArea);
                await LoadMapAreaAsync((int)MapArea.PlainArea, MapArea.PlainArea);
                UnloadMapArea(MapArea.PlagueDoctorArea);
                await LoadMapAreaAsync((int)MapArea.MutantArea, MapArea.MutantArea);
                break;
        }

        ShowEnemy(mapAreaTriggerid, areaEnemyDictionary[mapAreaTriggerid]);

    }

    private void ShowEnemy(int mapAreaTriggerid, List<GameObject> enemyList = null)
    {
        if (fullEnemyList.Count < 1)
        {
            Log.Error("場景全敵人List為空!");
            return;
        }
        else if (enemyList.Count < 1)
        {
            Log.Error("區域敵人List為空!");
            return;
        }

        var enemyCompareResultList =  GameObjectComparer.CompareAndExecuteMethods(fullEnemyList, enemyList);
        SetActiveEnemy(enemyCompareResultList.commonElementsList, true);
        SetActiveEnemy(enemyCompareResultList.differentElementsList, false);

    }

    /// <summary>
    /// 載入地圖區域
    /// </summary>
    /// <param name="index">地圖區域編號</param>
    /// <param name="mapAreaType">地圖區域</param>
    private async Task LoadMapAreaAsync(int index, MapArea mapAreaType)
    {
        try
        {
            if (mapAreaDictionary[index] == null)
            {

                var mapAreaGameObject = await AddrssableAsync.LoadInstantiate(mapAreaType.ToString().ToLower(), this.transform);

                // 檢查操作是否成功
                if (mapAreaGameObject)
                {
                    mapAreaDictionary[index] = mapAreaGameObject;
                }
                else
                {
                    // 處理載入失敗的情況
                    Log.Error($"無法載入或生成 asset: {mapAreaGameObject}");
                }
            }
        }
        catch (Exception ex)
        {
            // 處理異常情況
            Log.Error($"錯誤: {ex.Message}");
        }
    }

    /// <summary>
    /// 卸載地圖區域
    /// </summary>
    /// <param name="index">地圖區域編號</param>
    /// <param name="mapAreaType">地圖區域</param>
    private void UnloadMapArea(MapArea mapAreaType)
    {
        if (mapAreaDictionary[(int)mapAreaType] != null)
        {
            AddrssableAsync.Unload(mapAreaDictionary[(int)mapAreaType]);
            mapAreaDictionary[(int)mapAreaType] = null;
        }
    }

    /// <summary>
    /// 開啟/關閉敵人
    /// </summary>
    /// <param name="enemyList">儲存敵人物件的List</param>
    /// <param name="isActive">是否開啟</param>
    public void SetActiveEnemy(List<GameObject> enemyList, bool isActive)
    {
        if (enemyList.Count > 0)
        {
            foreach (var enemyGameObject in enemyList)
            {
                enemyGameObject.SetActive(isActive);
            }
        }
    }

    #endregion
}
