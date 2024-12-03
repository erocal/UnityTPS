using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class BuildInformation : MonoBehaviour
{
    #region -- 參數參考區 --

    public static string Version { get; set; }
    public static string ScriptingBackend { get; set; }
    public static string Define { get; set; }
    public static string AppInfo { get; set; }
    public static string ProjectFolderName { get; set; }
    public static string BundleVersionCode { get; set; }
    public static string TargetAPILevel { get; set; }

    #endregion

    #region -- 方法參考區 --

    [MenuItem("Tools/包版資訊/全部資訊", false, 581)]
    public static void ShowBuildInformation()
    {
        ShowVersion();
        ShowScriptingBackend();
        //ShowAppInfoAndDefine();
        ShowProjectFolderName();
#if UNITY_ANDROID
            ShowBundleVersionCode();
            ShowTargetAPILevel();
#endif
    }

    /// <summary>
    /// 顯示應用程式版本號
    /// </summary>
    [MenuItem("Tools/包版資訊/Version", false, 592)]
    static void ShowVersion()
    {
        // 獲取應用程式版本號
        string appVersion = Application.version;

        Version = $"應用程式版本號: {appVersion}";

        // 列印版本號到控制台
        Debug.Log(Version);
    }

    /// <summary>
    /// 顯示scriptingBackend模式
    /// </summary>
    [MenuItem("Tools/包版資訊/Scripting Backend", false, 593)]
    static void ShowScriptingBackend()
    {

        // 獲取scriptingBackend模式
        ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(NamedBuildTarget.Standalone);

        ScriptingBackend = $"Scripting Backend: {scriptingBackend}";

        // 印出scriptingBackend模式
        Debug.Log(ScriptingBackend);
    }

    /// <summary>
    /// 顯示目前伺服器和是否在Debug
    /// </summary>
    //[MenuItem("Tools/包版資訊/AppInfo Define", false, 594)]
    //static void ShowAppInfoAndDefine()
    //{

    //    Define = $"目前Debug is {ETModel.Define.IsDebug}";
    //    AppInfo = $"你正在伺服器: {Appinfo.GetServerConfigUrl()}";

    //    // 查看是否為Debug模式
    //    Debug.Log(Define);

    //    // 查看目前伺服器
    //    Debug.Log(AppInfo);
    //}

    /// <summary>
    /// 顯示目前開啟專案資料夾名稱
    /// </summary>
    [MenuItem("Tools/包版資訊/Project FolderName", false, 595)]
    static void ShowProjectFolderName()
    {
        string dataPath = Application.dataPath;
        string[] splitPath = dataPath.Split('/');
        string projectFolderName = splitPath[splitPath.Length - 3];

        ProjectFolderName = $"目前專案資料夾名稱: {projectFolderName}";

        Debug.Log(ProjectFolderName);
    }

#if UNITY_ANDROID

        /// <summary>
        /// 顯示Bundle Version Code
        /// </summary>
        [MenuItem("Tools/包版資訊/Bundle Version Code", false, 596)]
        static void ShowBundleVersionCode()
        {
            // 獲取Bundle Version Code
            int bundleVersionCode = PlayerSettings.Android.bundleVersionCode;

            BundleVersionCode = $"Bundle Version Code: {bundleVersionCode}";

            // 印出Bundle Version Code
            Debug.Log(BundleVersionCode);
        }

        /// <summary>
        /// 顯示Target API Level
        /// </summary>
        [MenuItem("Tools/包版資訊/Target API Level", false, 597)]
        static void ShowTargetAPILevel()
        {
            // 獲取Target API Level
            var targetApiLevel = PlayerSettings.Android.targetSdkVersion;

            TargetAPILevel = $"Target API Level: {targetApiLevel}";

            // 印出Target API Level
            Debug.Log(TargetAPILevel);
        }

#endif

    #endregion

}
