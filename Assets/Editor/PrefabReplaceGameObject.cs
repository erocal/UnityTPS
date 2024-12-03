using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabReplaceGameObject : EditorWindow
{

    #region -- 資源參考區 --

    public GUISkin evanGUISkin;
    public GUISkin defaultGUISkin;

    #endregion

    #region -- 變數參考區 --

    private GameObject prefab;
    private GameObject parentObject;
    private List<GameObject> gameObjectsList = new List<GameObject>();
    private Vector2 scrollPosition;

    #endregion

    #region -- 初始化/運作 --

    private void OnGUI()
    {

        //設置Skin
        GUI.skin = this.evanGUISkin;

        //整體Windows
        GUILayout.BeginVertical(evanGUISkin.window);
        {

            GUILayout.Label("Prefab Replace GameObject", evanGUISkin.customStyles[GUISkinEvan_Style.Title]);

            DragAndDropUtility.DrawDragAndDropArea(ref gameObjectsList, ref scrollPosition);

            GUILayout.Space(20);

            // 選擇Prefab
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

            // 選擇父物件
            parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

            GUILayout.Space(20);

            // 生成按鈕
            if (GUILayout.Button("Generate Prefabs"))
            {
                GeneratePrefabs();
            }

        }
        GUILayout.EndVertical();

    }

    #endregion

    #region -- 方法參考區 --

    [MenuItem("Tools/用Prefab替換物件但保留Transform")]
    public static void ShowWindow()
    {
        GetWindow<PrefabReplaceGameObject>("Prefab Replace GameObject");
    }

    private void GeneratePrefabs()
    {
        // 檢查輸入是否正確
        if (prefab == null)
        {
            Debug.LogError("請選擇一個Prefab！");
            return;
        }

        if (parentObject == null)
        {
            Debug.LogError("請選擇一個父物件！");
            return;
        }

        if (gameObjectsList.Count == 0)
        {
            Debug.LogError("沒有選擇任何物件!");
            return;
        }


        // 開始生成
        foreach (GameObject obj in gameObjectsList)
        {
            if (obj == null)
            {
                Debug.LogWarning("忽略了空的GameObject引用！");
                continue;
            }

            // 實例化Prefab並設置父物件
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.SetParent(parentObject.transform);

            // 複製Transform值
            instance.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);
            instance.transform.localScale = obj.transform.localScale;

            Debug.Log($"生成Prefab {instance.name} 並賦值Transform自 {obj.name}");

            DestroyImmediate(obj);
            
        }

        Debug.Log("Prefab生成完成！");
    }

    #endregion

}