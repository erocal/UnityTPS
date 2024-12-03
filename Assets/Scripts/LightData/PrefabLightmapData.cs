using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PrefabLightmapData : MonoBehaviour
{

    #region -- 資源參考區 --

    [Tooltip("Reassigns shaders when applying the baked lightmaps. Might conflict with some shaders like transparent HDRP.")]
    public bool releaseShaders = true;

    [System.Serializable]
    public struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }
    [System.Serializable]
    public struct LightInfo
    {
        public Light light;
        public int lightmapBaketype;
        public int mixedLightingMode;
    }

    [SerializeField]
    RendererInfo[] m_RendererInfo;
    [SerializeField]
    Texture2D[] m_Lightmaps;
    [SerializeField]
    Texture2D[] m_LightmapsDir;
    [SerializeField]
    Texture2D[] m_ShadowMasks;
    [SerializeField]
    LightInfo[] m_LightInfo;

    public RendererInfo[] RendererInformation
    {
        get
        {
            return m_RendererInfo;
        }
        set
        {
            m_RendererInfo = value;
        }
    }

    public Texture2D[] Lightmaps
    {
        get
        {
            return m_Lightmaps;
        }
        set
        {
            m_Lightmaps = value;
        }
    }

    public Texture2D[] LightmapsDir
    {
        get
        {
            return m_LightmapsDir;
        }
        set
        {
            m_LightmapsDir = value;
        }
    }

    public Texture2D[] ShadowMasks
    {
        get
        {
            return m_ShadowMasks;
        }
        set
        {
            m_ShadowMasks = value;
        }
    }

    public LightInfo[] LightInformation
    {
        get
        {
            return m_LightInfo;
        }
        set
        {
            m_LightInfo = value;
        }
    }

    #endregion

    #region -- 初始化/運作 --

    void Awake()
    {
        Init();
    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            return;

        var lightmaps = LightmapSettings.lightmaps;
        int[] offsetsindexes = new int[m_Lightmaps.Length];
        int counttotal = lightmaps.Length;
        List<LightmapData> combinedLightmaps = new List<LightmapData>();

        for (int i = 0; i < m_Lightmaps.Length; i++)
        {
            bool exists = false;
            for (int j = 0; j < lightmaps.Length; j++)
            {

                if (m_Lightmaps[i] == lightmaps[j].lightmapColor)
                {
                    exists = true;
                    offsetsindexes[i] = j;

                }

            }
            if (!exists)
            {
                offsetsindexes[i] = counttotal;
                var newlightmapdata = new LightmapData
                {
                    lightmapColor = m_Lightmaps[i],
                    lightmapDir = m_LightmapsDir.Length == m_Lightmaps.Length ? m_LightmapsDir[i] : default,
                    shadowMask = m_ShadowMasks.Length == m_Lightmaps.Length ? m_ShadowMasks[i] : default,
                };

                combinedLightmaps.Add(newlightmapdata);

                counttotal += 1;


            }

        }

        var combinedLightmaps2 = new LightmapData[counttotal];

        lightmaps.CopyTo(combinedLightmaps2, 0);
        combinedLightmaps.ToArray().CopyTo(combinedLightmaps2, lightmaps.Length);

        bool directional = true;

        foreach (Texture2D t in m_LightmapsDir)
        {
            if (t == null)
            {
                directional = false;
                break;
            }
        }

        LightmapSettings.lightmapsMode = (m_LightmapsDir.Length == m_Lightmaps.Length && directional) ?
            LightmapsMode.CombinedDirectional : LightmapsMode.NonDirectional;
        ApplyRendererInfo(m_RendererInfo, offsetsindexes, m_LightInfo);
        LightmapSettings.lightmaps = combinedLightmaps2;
    }

    /// <summary>
    /// 應用LightingMap
    /// </summary>
    void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapOffsetIndex, LightInfo[] lightsInfo)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            var info = infos[i];

            info.renderer.lightmapIndex = lightmapOffsetIndex[info.lightmapIndex];
            info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;

            if (releaseShaders)
            {
                // You have to release shaders.
                Material[] mat = info.renderer.sharedMaterials;
                for (int j = 0; j < mat.Length; j++)
                {
                    if (mat[j] != null && Shader.Find(mat[j].shader.name) != null)
                    {
                        mat[j].shader = Shader.Find(mat[j].shader.name);
                    }

                }
            }

        }

        for (int i = 0; i < lightsInfo.Length; i++)
        {
            LightBakingOutput bakingOutput = new LightBakingOutput();
            bakingOutput.isBaked = true;
            bakingOutput.lightmapBakeType = (LightmapBakeType)lightsInfo[i].lightmapBaketype;
            bakingOutput.mixedLightingMode = (MixedLightingMode)lightsInfo[i].mixedLightingMode;

            lightsInfo[i].light.bakingOutput = bakingOutput;

        }


    }

    #endregion

}