using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressablesBuild
{

    #region -- 變數參考區 --

    private static readonly string build_script_mode = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
    private static readonly string build_script_use_fastest_mode = "Assets/AddressableAssetsData/DataBuilders/BuildScriptFastMode.asset";
    private static readonly string build_script_use_exisiting_mode = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedPlayMode.asset";
    private static readonly string settings_asset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
    private static readonly string profile_android_name = "Android";
    private static readonly string profile_apk_name = "ForApk";

    private static readonly string message = "確定包版資訊無誤後再按下確認喔(^ω^) ! ";

    private static AddressableAssetSettings settings;

    #endregion

    [MenuItem("Tools/Addressables打包/Unity檢查模式", false, 561)]
    public static void UnityCheckMode()
    {

        if (SetBuilderFastestMode()) Debug.Log("Addressables Group更新為Unity檢查模式");

    }

#if UNITY_ANDROID

        [MenuItem("Tools/Addressables打包/打包Android", false, 562)]
        public static void AndroidPackageMode()
        {

            BuildInformation.ShowBuildInformation();
            AddressablesBuildWindow.Show(message, AndroidPackageModeOnConfirm, AndroidPackageModeOnCancel);

        }

#endif

    [MenuItem("Tools/Addressables打包/打包APK(iOS)", false, 563)]
    public static void APKPackageMode()
    {

        BuildInformation.ShowBuildInformation();
        AddressablesBuildWindow.Show(message, APKPackageModeOnConfirm, APKPackageModeOnCancel);


    }

    #region -- 方法參考區 --

    /// <summary>
    /// Addressable打包: Andorid
    /// </summary>
    private static void AndroidPackageModeOnConfirm(bool isClear)
    {
        if (isClear) BuildClear.ClearAndroid();
        SetBuilderExisitingMode();
        SetProfile(profile_android_name);
        if (BuildAddressableContent()) UnityCheckMode();
    }

    /// <summary>
    /// Addressable取消打包: Andorid
    /// </summary>
    private static void AndroidPackageModeOnCancel()
    {
        Debug.Log("Android打包程序中止");
    }

    /// <summary>
    /// Addressable打包: APK
    /// </summary>
    private static void APKPackageModeOnConfirm(bool isClear)
    {
        if (isClear) BuildClear.CleariOSAndAPK();
        SetBuilderExisitingMode();
        SetProfile(profile_apk_name);
        if (BuildAddressableContent()) UnityCheckMode();
    }

    /// <summary>
    /// Addressable取消打包: APK
    /// </summary>
    private static void APKPackageModeOnCancel()
    {
        Debug.Log("APK打包程序中止");
    }

    /// <summary>
    /// 設定Addressable打包使用的DataBuilder
    /// </summary>
    private static void SetBuilderPackageMode()
    {

        GetSettingsObject(settings_asset);
        IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script_mode) as IDataBuilder;

        if (builderScript == null)
        {
            Debug.LogError($"{build_script_mode} 找不到或傳入並非正確DataBuilders");
            return;
        }

        SetBuilder(builderScript);

    }

    /// <summary>
    /// 設定Addressable Groups 的 Play Mode Script 為 Use Asset Database (fastest)
    /// </summary>
    private static bool SetBuilderFastestMode()
    {

        GetSettingsObject(settings_asset);
        IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script_use_fastest_mode) as IDataBuilder;

        if (builderScript == null)
        {
            Debug.LogError($"{build_script_use_fastest_mode} 找不到或傳入並非正確DataBuilders");
            return false;
        }

        SetPlayModeDataBuilder(builderScript);

        return true;

    }

    /// <summary>
    /// 設定Addressable Groups 的 Play Mode Script 為 Use Existing Build (requires built groups)
    /// </summary>
    private static void SetBuilderExisitingMode()
    {

        GetSettingsObject(settings_asset);
        IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script_use_exisiting_mode) as IDataBuilder;

        if (builderScript == null)
        {
            Debug.LogError($"{build_script_use_exisiting_mode} 找不到或傳入並非正確DataBuilders");
            return;
        }

        SetPlayModeDataBuilder(builderScript);

    }

    /// <summary>
    /// 設置AddressableAssetSettings
    /// </summary>
    static void GetSettingsObject(string settingsAsset)
    {

        settings = AddressableAssetSettingsDefaultObject.Settings;

        settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset) as AddressableAssetSettings;

        if (settings == null) Debug.LogError($"{settingsAsset} 找不到或傳入並非正確settings asset");
    }

    /// <summary>
    /// 設置Addressable Groups Profile
    /// </summary>
    /// <param name="profile"></param>
    static void SetProfile(string profile)
    {
        string profileId = settings.profileSettings.GetProfileId(profile);
        if (String.IsNullOrEmpty(profileId)) Debug.LogError($"找不到此{profile}命名的profile, profile將不會變更");
        else settings.activeProfileId = profileId;
    }

    /// <summary>
    /// 設置AddressableAssetSettings打包所使用的DataBuilder
    /// </summary>
    static void SetBuilder(IDataBuilder builder)
    {
        int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

        if (index > -1)
        {
            settings.ActivePlayerDataBuilderIndex = index;
        }
        else Debug.LogWarning($"{settings} 的 Build and Play Mode Scripts 找不到 {builder}, PlayerDataBuilder將不會變更");
    }

    /// <summary>
    /// 設置Addressable Groups 的 Play Mode Script 所對應的DataBuilder
    /// </summary>
    static void SetPlayModeDataBuilder(IDataBuilder builder)
    {
        int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

        if (index > -1)
        {
            settings.ActivePlayModeDataBuilderIndex = index;
        }
        else Debug.LogWarning($"{settings} 的 Build and Play Mode Scripts 找不到 {builder}, PlayerModeDataBuilder將不會變更");
    }

    /// <summary>
    /// Addressable System打包
    /// </summary>
    /// <returns></returns>
    static bool BuildAddressableContent()
    {

        SetBuilderPackageMode();
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (!success)
        {
            Debug.LogError("Addressables 建置錯誤: " + result.Error);
        }
        return success;

    }

    #endregion

}
