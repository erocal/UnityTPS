using UnityEditor;
using UnityEngine;
using System;

public class AddressablesBuildWindow : EditorWindow
{

    #region -- 資源參考區 --

    public GUISkin evanGUISkin;
    public GUISkin defaultGUISkin;

    #endregion

    #region -- 變數參考區 --

    public static AddressablesBuildWindow addressablesBuildWindow;

    private string message;
    private bool isClear = true;
    private Action<bool> onConfirm;
    private Action onCancel;

    #endregion

    #region -- 初始化/運作 --

    private void OnGUI()
    {

        //設置Skin
        GUI.skin = this.evanGUISkin;

        //整體Windows
        GUILayout.BeginVertical(evanGUISkin.window);
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(evanGUISkin.box);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(message, evanGUISkin.customStyles[GUISkinEvan_Style.Title]);
                        if (GUILayout.Button("刷新", evanGUISkin.customStyles[GUISkinEvan_Style.Button_Blue]))
                        {

                            BuildInformation.ShowBuildInformation();

                            Repaint();
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(20);

                    ShowBuildInformation();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("是否清除包版對應路徑Addressable Assets資料");
                        isClear = GUILayout.Toggle(isClear, string.Empty, evanGUISkin.toggle);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(20);

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("確認"))
                    {
                        onConfirm?.Invoke(isClear);
                        addressablesBuildWindow.Close();
                    }
                    if (GUILayout.Button("取消"))
                    {
                        onCancel?.Invoke();
                        addressablesBuildWindow.Close();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

    }

    #endregion

    #region -- 方法參考區 --

    public static void Show(string message, Action<bool> onConfirm, Action onCancel)
    {
        addressablesBuildWindow = GetWindow<AddressablesBuildWindow>(true);
        addressablesBuildWindow.message = message;
        addressablesBuildWindow.onConfirm = onConfirm;
        addressablesBuildWindow.onCancel = onCancel;
        addressablesBuildWindow.Show();
    }

    private void ShowBuildInformation()
    {

        GUILayout.BeginVertical(evanGUISkin.box);
        {

            GUILayout.Label("Version : " + BuildInformation.Version + "\n" +
                            BuildInformation.ScriptingBackend + "\n" +
                            "Define : " + BuildInformation.Define + "\n" +
                            "AppInfo : " + BuildInformation.AppInfo + "\n" +
                            "ProjectFolderName : " + BuildInformation.ProjectFolderName + "\n" +
                            "BundleVersionCode : " + BuildInformation.BundleVersionCode + "\n" +
                            "TargetAPILevel : " + BuildInformation.TargetAPILevel + "\n");

        }
        GUILayout.EndVertical();

    }

    #endregion

}
