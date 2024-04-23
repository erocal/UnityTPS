using UnityEngine;
using UnityEditor;

public class MeshRendererAfterBakedLightEditor : EditorWindow
{

    [MenuItem("Tools/烘培光後/MeshRenderer/Cast Shadows設定為Off", false, 1)]
    static void CastShadowsTurnOff()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        Debug.Log("場景全部物件MeshRenderer Cast Shadows設定為Off成功!");
    }

    [MenuItem("Tools/烘培光後/MeshRenderer/取消接收陰影", false, 2)]
    static void DisableReceiveShadows()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.receiveShadows = false;
        }

        Debug.Log("場景全部物件MeshRenderer取消接收陰影成功");
    }

    [MenuItem("Tools/烘培光後/MeshRenderer/LightProbes設定為Off", false, 3)]
    static void LightProbesTurnOff()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        }

        Debug.Log("場景全部物件MeshRenderer LightProbes設定為Off成功!");
    }

    [MenuItem("Tools/烘培光後/MeshRenderer/ReflectionProbes設定為Off", false, 4)]
    static void ReflectionProbesTurnOff()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        }

        Debug.Log("場景全部物件MeshRenderer ReflectionProbes設定為Off成功!");
    }

    [MenuItem("Tools/烘培光後/MeshRenderer/Force No Motion", false, 5)]
    static void ForceNoMotion()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        }

        Debug.Log("場景全部物件MeshRenderer Motion Vectors 設定為Force No Motion");
    }

    [MenuItem("Tools/烘培光後/MeshRenderer/取消動態剔除", false, 6)]
    static void DisableOcclusion()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.allowOcclusionWhenDynamic = false;
        }

        Debug.Log("場景全部物件MeshRenderer取消動態剔除成功!");
    }

}
