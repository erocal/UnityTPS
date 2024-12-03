using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class DragAndDropUtility
{
    public static void DrawDragAndDropArea(ref List<GameObject> gameObjectsList, ref Vector2 scrollPosition)
    {
        GUILayout.Label("拖曳 GameObjects 到這個區域", EditorStyles.boldLabel);

        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "拖曳 GameObjects 到這裡");

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (UnityEngine.Object draggedObject in DragAndDrop.objectReferences)
                    {
                        GameObject draggedGameObject = draggedObject as GameObject;
                        if (draggedGameObject != null && !gameObjectsList.Contains(draggedGameObject))
                        {
                            gameObjectsList.Add(draggedGameObject);
                        }
                    }
                }
                Event.current.Use();
                break;
        }

        GUILayout.Label("已儲存的 GameObjects：", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));

        for (int i = 0; i < gameObjectsList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            gameObjectsList[i] = (GameObject)EditorGUILayout.ObjectField(gameObjectsList[i], typeof(GameObject), true);

            if (GUILayout.Button("移除", GUILayout.Width(60)))
            {
                gameObjectsList.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        if (GUILayout.Button("清空清單"))
        {
            gameObjectsList.Clear();
            Debug.Log("gameObjectsList 已清空");
        }
    }
}
