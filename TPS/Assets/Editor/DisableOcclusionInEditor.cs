using UnityEngine;
using UnityEditor;

public class DisableOcclusionInEditor : EditorWindow
{
    [MenuItem("Tools/�M������/������������MeshRenderer�ʺA�簣")]
    static void DisableOcclusion()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.allowOcclusionWhenDynamic = false;
        }

        Debug.Log("������������MeshRenderer�����ʺA�簣���\!");
    }
}
