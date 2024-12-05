using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static PrefabLightmapData;

public class GenerateLightingWithPrefabLightmapData : MonoBehaviour
{

    // 定義 Lightmap 信息的結構
    class LightmapInfo
    {
        public Texture2D LightmapColor { get; set; }
        public Texture2D LightmapDir { get; set; }
        public Texture2D ShadowMask { get; set; }
    }

    // 存儲 renderer 數據的結構
    class RendererData
    {
        public MeshRenderer Renderer { get; set; }
        public int LightmapIndex { get; set; }
        public Vector4 LightmapScaleOffset { get; set; }
    }

    // 存儲 Prefab 資料的結構
    class PrefabData
    {
        public PrefabLightmapData PrefabInstance { get; set; }
        public List<RendererData> RendererData { get; set; }
        public List<Light> Lights { get; set; }
    }

    [MenuItem("Assets/Generate Lighting With Prefab")]
    public static void GenerateLightmapInfo()
    {
        Lightmapping.Bake();

        // 主線程收集 LightmapSettings 數據
        LightmapData[] lightmaps = LightmapSettings.lightmaps;
        var lightmapInfos = new List<LightmapInfo>();

        foreach (var lightmap in lightmaps)
        {
            lightmapInfos.Add(new LightmapInfo
            {
                LightmapColor = lightmap.lightmapColor,
                LightmapDir = lightmap.lightmapDir,
                ShadowMask = lightmap.shadowMask
            });
        }

        var prefabs = FindObjectsByType<PrefabLightmapData>(FindObjectsSortMode.InstanceID);
        var dataList = new List<PrefabData>();

        foreach (var instance in prefabs)
        {

            var renderers = instance.gameObject.GetComponentsInChildren<MeshRenderer>();
            var lights = instance.gameObject.GetComponentsInChildren<Light>(true);

            var rendererData = renderers
                .Select(renderer => new RendererData
                {
                    Renderer = renderer,
                    LightmapIndex = renderer.lightmapIndex,
                    LightmapScaleOffset = renderer.lightmapScaleOffset
                })
                .ToList();

            dataList.Add(new PrefabData
            {
                PrefabInstance = instance,
                RendererData = rendererData,
                Lights = lights.ToList()
            });

        }

        // 並行處理數據
        var lightmapsDir = new ConcurrentBag<Texture2D>();
        var shadowMasks = new ConcurrentBag<Texture2D>();
        var lightsInfos = new ConcurrentBag<PrefabLightmapData.LightInfo>();

        Parallel.ForEach(dataList, data =>
        {
            ProcessLightmapData(data, lightmapInfos, lightsInfos);
        });

        // 在主線程應用數據
        foreach (var data in dataList)
        {

            var instance = data.PrefabInstance;

            var rendererInfos = new List<PrefabLightmapData.RendererInfo>();
            var lightmap = new HashSet<Texture2D>();

            // 優化數據處理，避免重複添加
            GenerateRendererInfo(instance.gameObject, rendererInfos, lightmap);

            instance.RendererInformation = rendererInfos.ToArray();
            instance.Lightmaps = lightmapInfos.Select(info => info.LightmapColor).ToArray();
            instance.LightmapsDir = lightmapInfos.Select(info => info.LightmapDir).ToArray();
            instance.ShadowMasks = lightmapInfos.Select(info => info.ShadowMask).ToArray();
            instance.LightInformation = lightsInfos.ToArray();

            ApplyPrefabChanges(instance);
        }

        Debug.Log("Lighting generation completed successfully.");

    }

    static void ProcessLightmapData(PrefabData data,
                                List<LightmapInfo> lightmapInfos,
                                ConcurrentBag<PrefabLightmapData.LightInfo> lightsInfo)
    {

        foreach (var light in data.Lights)
        {
            var lightInfo = new PrefabLightmapData.LightInfo
            {
                light = light,
                lightmapBaketype = (int)light.lightmapBakeType
            };

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


    static void ApplyPrefabChanges(PrefabLightmapData instance)
    {
#if UNITY_2018_3_OR_NEWER
        var targetPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance.gameObject) as GameObject;

        if (targetPrefab != null)
        {
            var root = PrefabUtility.GetOutermostPrefabInstanceRoot(instance.gameObject);
            if (root != null)
            {
                var rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance.gameObject);
                var rootPath = AssetDatabase.GetAssetPath(rootPrefab);

                PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, PrefabUnpackMode.OutermostRoot);

                try
                {
                    PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
                }
                catch { }
                finally
                {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);
                }
            }
            else
            {
                PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
            }
        }
#else
        var targetPrefab = PrefabUtility.GetPrefabParent(instance.gameObject) as GameObject;
        if (targetPrefab != null)
        {
            PrefabUtility.ReplacePrefab(instance.gameObject, targetPrefab);
        }
#endif
    }

    static void GenerateRendererInfo(
        GameObject root,
        List<PrefabLightmapData.RendererInfo> rendererInfos,
        HashSet<Texture2D> lightmaps)
    {
        var renderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            if (renderer.lightmapIndex != -1 && renderer.lightmapScaleOffset != Vector4.zero)
            {
                PrefabLightmapData.RendererInfo info = new PrefabLightmapData.RendererInfo
                {
                    renderer = renderer,
                    lightmapOffsetScale = renderer.lightmapScaleOffset
                };

                if (renderer.lightmapIndex >= 0 && renderer.lightmapIndex != 0xFFFE)
                {

                    var lightmapData = LightmapSettings.lightmaps[renderer.lightmapIndex];
                    info.lightmapIndex = AddLightmap(lightmapData.lightmapColor, lightmaps);

                }

                rendererInfos.Add(info);
            }
        }
    }

    static int AddLightmap(Texture2D lightmap, HashSet<Texture2D> lightmaps)
    {
        if (lightmap == null || lightmaps.Contains(lightmap)) return -1;
        lightmaps.Add(lightmap);
        return lightmaps.Count - 1;
    }
}
