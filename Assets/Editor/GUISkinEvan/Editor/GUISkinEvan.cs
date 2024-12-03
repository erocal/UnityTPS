using UnityEditor;
using UnityEngine;


public class GUISkinEvan_Style
{
    public const int Title                = 0;
    public const int Button_Blue          = 1;
    public const int Popup                = 2;
    public const int List_Button          = 3;
    public const int List_Button_Press    = 4;
}

public class GUISkinEvan : EditorWindow
{
    public GUISkin evanGUISkin;
    public GUISkin defaultGUISkin;

    private float _horizontalSliderValue = 0.0f;
    private float _verticalSliderValue = 0.0f;
    private Vector2 _scrollPos = Vector2.zero;
    private string _textFieldContext = "輸入框";
    private bool _toggleValue = false;
    private int _listIndex = -1;
    private int _popupIndex = 0;
    private string[] _popupNameArray = new string[]
    {
        "下拉選單 1",
        "下拉選單 2",
        "下拉選單 3",
        "下拉選單 4",
        "下拉選單 5",
        "下拉選單 6",
        "下拉選單 7",
        "下拉選單 8",
        "下拉選單 9",
        "下拉選單 10",
    };

    [MenuItem("Tools/GUI樣板")]
    static void OpenEditor()
    {
        EditorWindow.GetWindow<GUISkinEvan>(true);
        EditorApplication.isPlaying = true;
    }

    bool havePlayModeStateChanged = false;
    bool isPlaying = false;

    void Init()
    {
        isPlaying = true;
    }

    public void PlayModeStateChange(PlayModeStateChange playModeStateChange)
    {
        Debug.Log(playModeStateChange);
        switch (playModeStateChange)
        {
            case UnityEditor.PlayModeStateChange.ExitingPlayMode:
                if (isPlaying)
                {
                    isPlaying = false;
                    this.Close();
                }
                break;
            case UnityEditor.PlayModeStateChange.EnteredPlayMode:
                Init();
                break;
        }
    }

    void Update()
    {
        Repaint();
    }

    void OnDestroy()
    {
        isPlaying = false;
        EditorApplication.isPlaying = false;
    }

    void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            GUILayout.Label("請在PlayMode下執行編輯器...");
            return;
        }

        if (!havePlayModeStateChanged)
        {
            EditorApplication.playModeStateChanged += PlayModeStateChange;
            havePlayModeStateChanged = true;
            return;
        }

        if (!isPlaying)
        {
            return;
        }

        //設置Skin
        GUI.skin = this.evanGUISkin;

        //整體Windows
        GUILayout.BeginVertical(evanGUISkin.window);
        {
            //上區域
            GUILayout.BeginHorizontal();
            {
                //說明
                GUILayout.BeginVertical(evanGUISkin.box);
                {
                    GUILayout.Label("標題1", evanGUISkin.customStyles[GUISkinEvan_Style.Title]);
                    GUILayout.Label("<color=#D30000FF>※※※※※※提示提示提示提示提示提示提示※※※※※※</color>\n" +
                                    "項目1 : 內文 <color=#DEA513FF>[異色]</color> 內文內文內文內文內文\n" +
                                    "項目2 : 內文 <color=#DEA513FF>[異色]</color> 內文內文內文（內文內文）\n" +
                                    "項目3 : 內文 <color=#DEA513FF>[異色]</color> 內文內文\n" +
                                    "項目4 : 內文 <color=#DEA513FF>[12]~[34]</color> 內文內文內文\n" +
                                    "項目5 : 內文 <color=#DEA513FF>[異色]</color> 內文內文內文內文\n" +
                                    "項目6 : 內文 <color=#DEA513FF>[異色]</color> 內文、內文、內文、內文\n" +
                                    "項目7 : <color=#DEA513FF>1.</color>縮排內文 <color=#DEA513FF>[ABC]</color>內文\n" +
                                    "        <color=#DEA513FF>2.</color>縮排內文 <color=#DEA513FF>[123]</color>內文\n"
                                    , evanGUISkin.label);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            //下區域
            GUILayout.BeginHorizontal();
            {
                //標題2
                GUILayout.BeginVertical(evanGUISkin.box, GUILayout.MaxWidth(this.position.width * 0.40f));
                {
                    GUILayout.Label("標題2", evanGUISkin.customStyles[GUISkinEvan_Style.Title]);

                    if (GUILayout.Button("按鈕1", evanGUISkin.button))
                    {
                        if (EditorUtility.DisplayDialog("跳窗1", "跳窗內文!!", "確定", "取消"))
                        {
                            Debug.Log("按下 跳窗1 確定");
                        }
                    }

                    GUILayout.Space(10);

                    _scrollPos = GUILayout.BeginScrollView(_scrollPos);
                    for (int i = 0; i < 10; i++)
                    {
                        if (GUILayout.Button("列表" + i, (_listIndex == i) ? evanGUISkin.customStyles[GUISkinEvan_Style.List_Button_Press] : evanGUISkin.customStyles[GUISkinEvan_Style.List_Button]))
                        {
                            _listIndex = i;
                            Debug.Log("選擇列表" + i);
                        }
                    }
                    GUILayout.EndScrollView();

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();

                //標題3
                GUILayout.BeginVertical(evanGUISkin.box);
                {
                    GUILayout.Label("標題3", evanGUISkin.customStyles[GUISkinEvan_Style.Title]);

                    if (GUILayout.Button("按鈕2", evanGUISkin.customStyles[GUISkinEvan_Style.Button_Blue]))
                    {
                        if (!EditorUtility.DisplayDialog("跳窗2", "跳窗2內文!!", "確定", "取消"))
                        {
                            Debug.Log("按下 跳窗2 取消");
                        }
                    }

                    GUILayout.Space(10);

                    _popupIndex = EditorGUILayout.Popup(_popupIndex, _popupNameArray, evanGUISkin.customStyles[GUISkinEvan_Style.Popup], GUILayout.Height(30));

                    GUILayout.Space(10);

                    _textFieldContext = GUILayout.TextField(_textFieldContext);

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Toggle");
                        _toggleValue = GUILayout.Toggle(_toggleValue, string.Empty, evanGUISkin.toggle);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    _horizontalSliderValue = GUILayout.HorizontalSlider(_horizontalSliderValue, 0, 100, evanGUISkin.horizontalSlider, evanGUISkin.horizontalSliderThumb);
                    _verticalSliderValue = GUILayout.VerticalSlider(_verticalSliderValue, 0, 100, evanGUISkin.verticalSlider, evanGUISkin.verticalSliderThumb);

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}