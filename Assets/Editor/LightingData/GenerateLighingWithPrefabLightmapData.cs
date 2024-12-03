using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateLighingWithPrefabLightmapData : MonoBehaviour
{

    public static void GenerateLightmapInfo()
    {

        Lightmapping.Bake();

        PrefabLightmapData[] prefabs = FindObjectsByType<PrefabLightmapData>(FindObjectsSortMode.InstanceID);

        foreach (var instance in prefabs)
        {
            var gameObject = instance.gameObject;
            var rendererInfos = new List<PrefabLightmapData.RendererInfo>();
            var lightmaps = new List<Texture2D>();
            var lightmapsDir = new List<Texture2D>();
            var shadowMasks = new List<Texture2D>();
            var lightsInfos = new List<PrefabLightmapData.LightInfo>();

            GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, lightmapsDir, shadowMasks, lightsInfos);

            instance.RendererInformation = rendererInfos.ToArray();
            instance.Lightmaps = lightmaps.ToArray();
            instance.LightmapsDir = lightmapsDir.ToArray();
            instance.LightInformation = lightsInfos.ToArray();
            instance.ShadowMasks = shadowMasks.ToArray();

#if UNITY_2018_3_OR_NEWER

            // 獲取當前物件對應的 Prefab 原始資源（非實例）
            var targetPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance.gameObject) as GameObject;

            // 如果有對應的 Prefab
            if (targetPrefab != null)
            {

                // 獲取最外層的 Prefab 實例根物件
                GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(instance.gameObject);

                // 如果存在最外層的 Prefab 實例根物件
                if (root != null)
                {
                    // 獲取當前物件的來源 Prefab 資源
                    GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance.gameObject);
                    // 獲取來源 Prefab 資源的路徑
                    string rootPath = AssetDatabase.GetAssetPath(rootPrefab);

                    // 解包最外層的 Prefab 實例並返回新的根物件
                    PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, PrefabUnpackMode.OutermostRoot);

                    try
                    {
                        // 將修改應用到當前物件所屬的 Prefab 實例
                        PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);

                    }
                    catch { }
                    finally
                    {
                        // 儲存解包後的物件為 Prefab 資源
                        PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);

                    }
                }
                else // 如果沒有最外層的 Prefab 實例根物件
                {
                    // 將修改應用到當前物件所屬的 Prefab 實例
                    PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
                }
            }
#else
            var targetPrefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
            if (targetPrefab != null)
            {
                //UnityEditor.Prefab
                UnityEditor.PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
            }
#endif
        }

    }

    static void GenerateLightmapInfo(GameObject root, List<PrefabLightmapData.RendererInfo> rendererInfos,
                                     List<Texture2D> lightmaps, List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks,
                                     List<PrefabLightmapData.LightInfo> lightsInfo)
    {
        var renderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.lightmapIndex != -1)
            {
                PrefabLightmapData.RendererInfo info = new PrefabLightmapData.RendererInfo();
                info.renderer = renderer;

                if (renderer.lightmapScaleOffset != Vector4.zero)
                {

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
            PrefabLightmapData.LightInfo lightInfo = new PrefabLightmapData.LightInfo();
            lightInfo.light = light;
            lightInfo.lightmapBaketype = (int)light.lightmapBakeType;
#if UNITY_2020_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)Lightmapping.lightingSettings.mixedBakeMode;
#elif UNITY_2018_1_OR_NEWER
            lightInfo.mixedLightingMode = (int)LightmapEditorSettings.mixedBakeMode;
#else
            lightInfo.mixedLightingMode = (int)light.bakingOutput.lightmapBakeType;            
#endif
            lightsInfo.Add(lightInfo);

        }
    }

}