using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public static class AddrssableAsync
{
    //addressable label的Key
    public const string AssetBundleDependicKey = "AllAsssetBundle";

    static AddrssableAsync()
    {
        Awake();
    }
    public static void Awake()
    {
        Addressables.InitializeAsync().WaitForCompletion();
        var updates = Addressables.CheckForCatalogUpdates().WaitForCompletion();
        if (updates != null)
        {
            if (updates.Count > 0)
                Addressables.UpdateCatalogs().WaitForCompletion();
        }

        //Addressables.InitializeAsync().Completed += (Result) =>
        //{
        //    Addressables.CheckForCatalogUpdates().WaitForCompletion();
        //    Addressables.UpdateCatalogs().WaitForCompletion();
        //    //await InitCompletedCallback();
        //};
    }
    private static async Task InitCompletedCallback()
    {
        var CheckResult = Addressables.CheckForCatalogUpdates();
        await CheckCataLogsCompleted(CheckResult);
    }
    private static async Task CheckCataLogsCompleted(AsyncOperationHandle<List<string>> checkCataLogsResult)
    {
        if (checkCataLogsResult.Result.Count > 0)
        {
            await UpdateCatalogs();
        }
    }



    public static Task UpdateCatalogs()
    {
        return Addressables.UpdateCatalogs().Task;
    }

    #region Load

    public static async Task<AsyncOperationHandle<T>> LoadAsync<T>(string assetName) where T : class
    {
        // 確保方法可正常進入異步模式
        await Task.CompletedTask;

        try
        {
            // 開始異步載入資源
            var handle = Addressables.LoadAssetAsync<T>(assetName);

            // 等待載入完成
            await handle.Task;

            // 檢查載入是否成功
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Log.Error($"載入資源失敗: {assetName}");
                return default;
            }
        }
        catch (System.Exception e)
        {
            // 捕獲例外並輸出錯誤訊息
            Log.Error($"載入資源過程中發生例外: {e.Message}");
            return default;
        }
    }

    public static async Task<GameObject> LoadInstantiate(string assetName, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
        try
        {
            return await Addressables.InstantiateAsync(assetName, parent, instantiateInWorldSpace, trackHandle).Task;
        }
        catch (System.Exception e)
        {
            Log.Error(e);
            return null;
        }

    }

    public static async Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        await Task.CompletedTask;
        try
        {
            return Addressables.LoadSceneAsync(sceneName, loadMode);
        }
        catch (System.Exception e)
        {
            Log.Error(e);
            return new AsyncOperationHandle<SceneInstance>();
        }
    }



    #endregion

    #region Unload
    public static void Unload(Object asset)
    {
        Addressables.Release(asset);
    }
    public static void Unload(AsyncOperationHandle<Object> async)
    {
        if (async.IsValid())
        {
            Addressables.Release(async);
        }
    }
    public static async void UnloadScene(AsyncOperationHandle<SceneInstance> async)
    {
        try
        {
            await Addressables.UnloadSceneAsync(async.Result).Task;
            return;
        }
        catch (System.Exception e)
        {
            Log.Error(e);
        }
    }

    #endregion

}
