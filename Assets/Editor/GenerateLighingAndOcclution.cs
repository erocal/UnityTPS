using UnityEditor;
using UnityEngine;

public class GenerateLighingAndOcclution
{

    #region -- 資源參考區 --

    #endregion

    #region -- 變數參考區 --

    #endregion

    #region -- 方法參考區 --

    [MenuItem("Assets/Generate Lighing And Occlution Data %&g")]
    static void GenerateLightmapAndOcclutionData()
    {

        GenerateLightingWithPrefabLightmapData.GenerateLightmapInfo();

        OcclusionBaking.BakeOcclusion();

    }

    #endregion

}