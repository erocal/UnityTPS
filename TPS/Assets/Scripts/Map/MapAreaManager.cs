using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum MapArea
{
    StartArea,
    PlainArea,
    PlagueDoctorArea,
    MutantArea
}

public class MapAreaManager : MonoBehaviour
{

    private Dictionary<int, GameObject> mapAreaDictionary = new Dictionary<int, GameObject>()
    {
        { 0, null},
        { 1, null},
        { 2, null},
        { 3, null},
        { 4, null},
    };

    public async void SwitchMapArea(int mapAreaTriggerid)
    {
        AsyncOperationHandle<GameObject> mapAreaGameObject;

        switch (mapAreaTriggerid)
        {
            case 0:
                await LoadMapAreaAsync((int)MapArea.StartArea, MapArea.StartArea);
                await LoadMapAreaAsync((int)MapArea.PlainArea, MapArea.PlainArea);
                UnloadMapArea(MapArea.PlagueDoctorArea);
                UnloadMapArea(MapArea.MutantArea);
                break;
            case 1:
                await LoadMapAreaAsync((int)MapArea.StartArea, MapArea.StartArea);
                await LoadMapAreaAsync((int)MapArea.PlainArea, MapArea.PlainArea);
                await LoadMapAreaAsync((int)MapArea.PlagueDoctorArea, MapArea.PlagueDoctorArea);
                UnloadMapArea(MapArea.MutantArea);
                break;
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
    }

    /// <summary>
    /// 載入地圖區域
    /// </summary>
    /// <param name="index">地圖區域編號</param>
    /// <param name="mapAreaType">地圖區域</param>
    /// <returns></returns>
    private async Task LoadMapAreaAsync(int index, MapArea mapAreaType)
    {
        if (mapAreaDictionary[index] == null)
        {
            var mapAreaGameObject = await AddrssableAsync.instance.LoadInstantiate(mapAreaType.ToString().ToLower(), this.transform);
            mapAreaDictionary[index] = mapAreaGameObject.Result;
        }
    }

    /// <summary>
    /// 卸載地圖區域
    /// </summary>
    /// <param name="index">地圖區域編號</param>
    /// <param name="mapAreaType">地圖區域</param>
    /// <returns></returns>
    private void UnloadMapArea(MapArea mapAreaType)
    {
        if (mapAreaDictionary[(int)mapAreaType] != null)
        {
            AddrssableAsync.instance.Unload(mapAreaDictionary[(int)mapAreaType]);
            mapAreaDictionary[(int)mapAreaType] = null;
        }
    }
}
