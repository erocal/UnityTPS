using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ETEditor
{
    public class AddressablesBuild
    {
        #region Addressable Building

        public static string build_script_use_fastest_mode = "Assets/AddressableAssetsData/DataBuilders/BuildScriptFastMode.asset";
        public static string build_script_use_exisiting_mode = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedPlayMode.asset";
        public static string settings_asset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        public static string profile_default_name = "Default";
        public static string profile_android_name = "Android";
        public static string profile_apk_name = "ForApk";
        private static AddressableAssetSettings settings;

        [MenuItem("Tools/Addressables打包/Unity檢查模式", false, 1)]
        public static void UnityCheckMode()
        {

            SetBuilderFastestMode();

        }

        [MenuItem("Tools/Addressables打包/打包Android模式", false, 2)]
        public static void AndroidPackageMode()
        {

            SetBuilderExisitingMode();
            SetProfile(profile_default_name);
            if(BuildAddressableContent()) UnityCheckMode();

        }

        [MenuItem("Tools/Addressables打包/打包APK模式", false, 3)]
        public static void APKPackageMode()
        {

            SetBuilderExisitingMode();
            SetProfile(profile_default_name);
            if (BuildAddressableContent()) UnityCheckMode();

        }

        public static void SetBuilderFastestMode()
        {

            GetSettingsObject(settings_asset);
            IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script_use_fastest_mode) as IDataBuilder;

            if (builderScript == null)
            {
                Debug.LogError(build_script_use_fastest_mode + " couldn't be found or isn't a build script.");
                return;
            }

            SetBuilder(builderScript);

        }

        public static void SetBuilderExisitingMode()
        {

            GetSettingsObject(settings_asset);
            IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script_use_exisiting_mode) as IDataBuilder;

            if (builderScript == null)
            {
                Debug.LogError(build_script_use_exisiting_mode + " couldn't be found or isn't a build script.");
                return;
            }

            SetBuilder(builderScript);

        }

        static void GetSettingsObject(string settingsAsset)
        {

            settings = AddressableAssetSettingsDefaultObject.Settings;

            settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsAsset) as AddressableAssetSettings;

            if (settings == null) Debug.LogError($"{settingsAsset} couldn't be found or isn't a settings object.");
        }

        static void SetProfile(string profile)
        {
            string profileId = settings.profileSettings.GetProfileId(profile);
            if (String.IsNullOrEmpty(profileId)) Debug.LogWarning($"Couldn't find a profile named, {profile}, using current profile instead.");
            else settings.activeProfileId = profileId;
        }

        static void SetBuilder(IDataBuilder builder)
        {
            int index = settings.DataBuilders.IndexOf((ScriptableObject)builder);

            if (index > -1)
            {
                settings.ActivePlayModeDataBuilderIndex = index;
                settings.ActivePlayerDataBuilderIndex = index;
            }
            else Debug.LogWarning($"{builder} must be added to the DataBuilders list before it can be made active. Using last run builder instead.");
        }

        static bool BuildAddressableContent()
        {

            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.LogError("Addressables build error encountered: " + result.Error);
            }
            return success;

        }
        #endregion
    }
}
