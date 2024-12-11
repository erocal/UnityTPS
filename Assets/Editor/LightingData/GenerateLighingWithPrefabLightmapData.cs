using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PrefabLightmapData;

public class GenerateLightingWithPrefabLightmapData : MonoBehaviour
{

    [MenuItem("Assets/Generate Lighting With Prefab")]
    public static void GenerateLightmapInfo()
    {
        Lightmapping.Bake();

        PrefabLightmapData[] prefabs = FindObjectsByType<PrefabLightmapData>(FindObjectsSortMode.InstanceID);
        HashSet<GameObject> roots = new HashSet<GameObject>();
        HashSet<GameObject> overridePrefabs = new HashSet<GameObject>();

        var rendererInfos = new List<RendererInfo>();
        var lightmaps = new List<Texture2D>();
        var lightmapsDir = new List<Texture2D>();
        var shadowMasks = new List<Texture2D>();
        var lightsInfos = new List<LightInfo>();

        foreach (var instance in prefabs)
        {

            instance.ResetResource();

            var gameObject = instance.gameObject;
            rendererInfos.Clear();
            lightmaps.Clear();
            lightmapsDir.Clear();
            shadowMasks.Clear();
            lightsInfos.Clear();

            GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, lightmapsDir, shadowMasks, lightsInfos);

            instance.RendererInformation = rendererInfos.ToArray();
            instance.Lightmaps = lightmaps.ToArray();
            instance.LightmapsDir = lightmapsDir.ToArray();
            instance.ShadowMasks = shadowMasks.ToArray();
            instance.LightInformation = lightsInfos.ToArray();

            GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);

            if (!roots.Contains(root))
            {
                roots.Add(root);
                overridePrefabs.Add(gameObject);
            }
            
        }

        foreach (GameObject gameObject in overridePrefabs)
        {

            OverridePrefabs(gameObject);

        }

    }

    static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps, List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks, List<LightInfo> lightsInfo)
    {
        var renderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.lightmapIndex != -1)
            {
                RendererInfo info = new RendererInfo();
                info.renderer = renderer;

                if (renderer.lightmapScaleOffset != Vector4.zero)
                {
                    //1ibrium's pointed out this issue : https://docs.unity3d.com/ScriptReference/Renderer-lightmapIndex.html
                    if (renderer.lightmapIndex < 0 || renderer.lightmapIndex == 0xFFFE) continue;
                    info.lightmapOffsetScale = renderer.lightmapScaleOffset;

                    Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
                    Texture2D lightmapDir = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapDir;
                    Texture2D shadowMask = LightmapSettings.lightmaps[renderer.lightmapIndex].shadowMask;

                    info.lightmapIndex = lightmaps.IndexOf(lightmap);
                    if (info.lightmapIndex == -1)
                    {
                        info.lightmapIndex = lightmaps.Count;
                        lightmaps.Add(lightmap);
                        lightmapsDir.Add(lightmapDir);
                        shadowMasks.Add(shadowMask);
                    }

                    rendererInfos.Add(info);
                }

            }
        }

        var lights = root.GetComponentsInChildren<Light>(true);

        foreach (Light light in lights)
        {
            LightInfo lightInfo = new LightInfo();
            lightInfo.light = light;
            lightInfo.lightmapBaketype = (int)light.lightmapBakeType;
#if UNITY_2020_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)UnityEditor.Lightmapping.lightingSettings.mixedBakeMode;
#elif UNITY_2018_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)UnityEditor.LightmapEditorSettings.mixedBakeMode;
#else
            lightInfo.mixedLightingMode = (int)light.bakingOutput.lightmapBakeType;
#endif
            lightsInfo.Add(lightInfo);

        }
    }

    /// <summary>
    /// 覆寫物件
    /// </summary>
    public static void OverridePrefabs(GameObject gameObject)
    {
#if UNITY_2018_3_OR_NEWER
        // 確保物件為有效的Prefab實例
        GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
        if (root == null)
        {
            Debug.LogWarning($"{gameObject.name} is not part of a Prefab instance.");
            return;
        }

        // 獲取Prefab原始資產
        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(root);
        if (sourcePrefab == null)
        {
            Debug.LogError("Failed to find corresponding Prefab source.");
            return;
        }

        string prefabPath = AssetDatabase.GetAssetPath(sourcePrefab);
        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("Failed to get Prefab path.");
            return;
        }

        // 應用變更並保存
        try
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(root, prefabPath, InteractionMode.AutomatedAction);
            Debug.Log($"Successfully updated Prefab at: {prefabPath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save Prefab: {ex.Message}");
        }
#else
        // Unity 2018.3 以下版本的邏輯
        var targetPrefab = PrefabUtility.GetPrefabParent(gameObject) as GameObject;
        if (targetPrefab != null)
        {
            PrefabUtility.ReplacePrefab(gameObject, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
            Debug.Log($"Replaced Prefab: {targetPrefab.name}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is not part of a Prefab instance.");
        }
#endif
    }

}
