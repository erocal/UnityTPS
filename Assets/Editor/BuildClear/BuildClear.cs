using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildClear : MonoBehaviour
{

    #region -- 參數參考區 --

    static readonly string assetpackPath = Path.GetDirectoryName(Application.dataPath) + "/Assets/StreamingAssets/assetpack";
    static readonly string androidPath = Path.GetDirectoryName(Application.dataPath) + "/Bundles/Android";
    static readonly string streamingAssetsCopyPath = Path.GetDirectoryName(Application.dataPath) + "/Library/StreamingAssetsCopy";
    static readonly string serverDataPath = Path.GetDirectoryName(Application.dataPath) + "/ServerData";

    #endregion

    #region -- 方法參考區 --

    [MenuItem("Tools/包版清除資料/Android", false, 571)]
    public static void ClearAndroid()
    {
        ClearassetpackPath();
        ClearAndroidPath();
        ClearStreamingAssetsCopyPath();
        ClearServerDataPath();

    }

    [MenuItem("Tools/包版清除資料/iOS 和 APK", false, 572)]
    public static void CleariOSAndAPK()
    {
        ClearassetpackPath();
        ClearServerDataPath();
    }

    [MenuItem("Tools/包版清除資料/個別清除/assetpack", false, 583)]
    /// <summary>
    /// 清空路徑 Unity\Assets\StreamingAssets\assetpack
    /// </summary>
    static void ClearassetpackPath()
    {
        if (Directory.Exists(assetpackPath))
        {
            // 刪除資料夾內的所有檔案
            foreach (string file in Directory.GetFiles(assetpackPath))
            {
                File.Delete(file);
            }

            // 刪除資料夾內的所有子資料夾
            foreach (string subfolder in Directory.GetDirectories(assetpackPath))
            {
                Directory.Delete(subfolder, true);
            }

            Debug.Log("assetpack資料夾內容已清空");
        }
        else
        {
            Debug.LogWarning("assetpack資料夾不存在");
        }
    }

    [MenuItem("Tools/包版清除資料/個別清除/Android", false, 583)]
    /// <summary>
    /// 清空路徑 Unity\Bundles\Android
    /// </summary>
    static void ClearAndroidPath()
    {
        if (Directory.Exists(androidPath))
        {
            // 刪除資料夾內的所有檔案
            foreach (string file in Directory.GetFiles(androidPath))
            {
                File.Delete(file);
            }

            // 刪除資料夾內的所有子資料夾
            foreach (string subfolder in Directory.GetDirectories(androidPath))
            {
                Directory.Delete(subfolder, true);
            }

            Debug.Log("Android資料夾內容已清空");
        }
        else
        {
            Debug.LogWarning("Android資料夾不存在");
        }
    }

    [MenuItem("Tools/包版清除資料/個別清除/StreamingAssetsCopy", false, 583)]
    /// <summary>
    /// 清空路徑 Unity\Library\StreamingAssetsCopy
    /// </summary>
    static void ClearStreamingAssetsCopyPath()
    {
        if (Directory.Exists(streamingAssetsCopyPath))
        {
            // 刪除資料夾內的所有檔案
            foreach (string file in Directory.GetFiles(streamingAssetsCopyPath))
            {
                File.Delete(file);
            }

            // 刪除資料夾內的所有子資料夾
            foreach (string subfolder in Directory.GetDirectories(streamingAssetsCopyPath))
            {
                Directory.Delete(subfolder, true);
            }

            Debug.Log("StreamingAssetsCopy資料夾內容已清空");
        }
        else
        {
            Debug.LogWarning("StreamingAssetsCopy資料夾不存在");
        }
    }

    [MenuItem("Tools/包版清除資料/個別清除/ServerData", false, 583)]
    /// <summary>
    /// 清空路徑 Unity\ServerData
    /// </summary>
    static void ClearServerDataPath()
    {
        if (Directory.Exists(serverDataPath))
        {
            // 刪除資料夾內的所有檔案
            foreach (string file in Directory.GetFiles(serverDataPath))
            {
                File.Delete(file);
            }

            // 刪除資料夾內的所有子資料夾
            foreach (string subfolder in Directory.GetDirectories(serverDataPath))
            {
                Directory.Delete(subfolder, true);
            }

            Debug.Log("ServerData資料夾內容已清空");
        }
        else
        {
            Debug.LogWarning("ServerData資料夾不存在");
        }
    }

    #endregion

}
