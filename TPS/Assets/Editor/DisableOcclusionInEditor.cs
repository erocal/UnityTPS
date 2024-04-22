using UnityEngine;
using UnityEditor;

public class DisableOcclusionInEditor : EditorWindow
{
    [MenuItem("Tools/烘培光後/取消場景物件MeshRenderer動態剔除")]
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
