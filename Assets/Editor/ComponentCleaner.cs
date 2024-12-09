using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ComponentCleaner : EditorWindow
{

    #region -- 資源參考區 --

    public GUISkin evanGUISkin;
    public GUISkin defaultGUISkin;

    #endregion

    #region -- 變數參考區 --

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

            GUILayout.Label("Remove Duplicate Components", evanGUISkin.customStyles[GUISkinEvan_Style.Title]);

            DragAndDropUtility.DrawDragAndDropArea(ref gameObjectsList, ref scrollPosition);

            GUILayout.Space(20);

            // 生成按鈕
            if (GUILayout.Button("Remove"))
            {
                RemoveDuplicateComponents();
            }

        }
        GUILayout.EndVertical();

    }

    #endregion

    #region -- 方法參考區 --

    [MenuItem("Tools/刪除重複的 Component")]
    public static void ShowWindow()
    {
        GetWindow<ComponentCleaner>("Component Cleaner");
    }

    private void RemoveDuplicateComponents()
    {

        foreach (GameObject gameObject in gameObjectsList)
        {

            // 取得所有 Component
            Component[] components = gameObject.GetComponents<Component>();
            var componentTypeSet = new HashSet<System.Type>();

            foreach (var component in components)
            {
                System.Type componentType = component.GetType();

                if (!componentTypeSet.Add(componentType))
                {
                    // 如果已經存在該類型，刪除此 Component
                    DestroyImmediate(component);
                }
            }

            EditorUtility.SetDirty(gameObject);

        }
    }

    #endregion

}