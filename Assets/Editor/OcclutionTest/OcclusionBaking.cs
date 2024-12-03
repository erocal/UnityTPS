using UnityEditor;
using UnityEngine;

public class OcclusionBaking
{

    #region -- 方法參考區 --

    public static void BakeOcclusion()
	{

        // 檢查當前場景是否有效
        if (string.IsNullOrEmpty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path))
        {
            Debug.LogError("請先保存場景再進行遮擋烘焙！");
            return;
        }

        // 執行遮擋烘焙
        try
        {
            Debug.Log("開始遮擋烘焙...");
            StaticOcclusionCulling.Compute();
            Debug.Log("遮擋烘焙完成！");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"遮擋烘焙失敗: {ex.Message}");
        }

    }

    #endregion

}