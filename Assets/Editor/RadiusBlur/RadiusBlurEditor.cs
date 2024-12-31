using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadiusBlur)), RequireComponent(typeof(Camera))]
public class RadiusBlurEditor : Editor
{

    #region -- 變數參考區 --

    RadiusBlur radiusBlur;

    public float centerX = 0.5f;
    public float centerY = 0.5f;
    [Range(0f, 0.03f)]
    public float radiusOffset = 0;
    [Range(1, 30)]
    public int iteration = 1;
    [Range(0f, 1.5f)]
    public float centerRange = 0f;
    bool first_loaded = true;

    private Camera targetCamera;
    private RenderTexture renderTexture;

    #endregion

    #region -- 初始化/運作 --

    private void OnEnable()
    {
        targetCamera = Camera.main;

        if (targetCamera != null)
        {
            renderTexture = new RenderTexture(1920, 1080, 16);
            targetCamera.targetTexture = renderTexture;
        }
    }

    private void OnDisable()
    {
        if (targetCamera != null)
        {
            targetCamera.targetTexture = null;
        }
        if (renderTexture != null)
        {
            renderTexture.Release();
            DestroyImmediate(renderTexture);
        }
    }
    public override void OnInspectorGUI()
    {

        if (targetCamera != null)
        {
            targetCamera.targetTexture = null;
            renderTexture.Release();
            DestroyImmediate(renderTexture);
            renderTexture = new RenderTexture(1920, 1080, 16);
            targetCamera.targetTexture = renderTexture;
        }

        radiusBlur = target as RadiusBlur;
        ShowRadiusData();
        first_loaded = false;

        ShowCamera();

        targetCamera.targetTexture = null;

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 逕向模糊數據
    /// </summary>
    void ShowRadiusData()
    {
        radiusBlur.radius_material = EditorGUILayout.ObjectField("徑向模糊材質", radiusBlur.radius_material, typeof(Material), true) as Material;
        GUILayout.Space(10);

        Vector3 radius_data = radiusBlur.radius_data;
        centerX = EditorGUILayout.Slider("徑向模糊中心X", centerX, 0f, 1f);
        centerY = EditorGUILayout.Slider("徑向模糊中心Y", centerY, 0f, 1f);
        // 跟徑向模糊中心分離開
        GUILayout.Space(10);
        radiusOffset = EditorGUILayout.Slider("偏移範圍(0~0.03)", radiusOffset, 0.0f, 0.03f);
        iteration = EditorGUILayout.IntSlider("迭代次數Iteration", iteration, 1, 30);
        centerRange = EditorGUILayout.Slider("模糊内徑", centerRange, 0f, 1.5f);

        if (first_loaded)
        {
            centerX = radius_data.x;
            centerY = radius_data.y;
            radiusOffset = radius_data.z;
            iteration = radiusBlur.iteration;
            centerRange = radiusBlur.radius_center_range;
        }
        if (first_loaded ||
            radius_data.x != centerX ||
            radius_data.y != centerY ||
            radius_data.z != radiusOffset ||
            radiusBlur.iteration != iteration ||
            radiusBlur.radius_center_range != centerRange)
        {
            radiusBlur.iteration = iteration;
            radiusBlur.radius_data = new Vector3(centerX, centerY, radiusOffset);
            radiusBlur.radius_center_range = centerRange;
        }
    }

    /// <summary>
    /// 顯示相機畫面
    /// </summary>
    private void ShowCamera()
    {

        if (targetCamera == null)
        {
            EditorGUILayout.HelpBox("No target camera found!", MessageType.Warning);
            return;
        }

        targetCamera.Render();

        float inspectorWidth = EditorGUIUtility.currentViewWidth - 20;

        GUILayout.Label("Camera Preview", EditorStyles.boldLabel);
        GUILayout.Box
        (
            renderTexture, 
            GUILayout.Width(inspectorWidth), 
            GUILayout.Height(inspectorWidth * .35f)
        );

    }

    #endregion

}
