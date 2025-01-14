/// sin 波
/// 請將參考物件掛在該Script的子物件
/// ※崩潰注意※

using UnityEngine;

public class VisableMathScript : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField] private Transform pointPrefab;
    [SerializeField, Range(10, 100)] private int resolution = 10;// 限制該值並顯示一個滑塊(10, 100)

    #endregion

    #region -- 變數參考區 --

    Transform[] points;

    #endregion

    #region -- 初始化/運作 --

    void Awake()
    {

        points = new Transform[resolution];
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        var position = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab); // 實例化方塊
            position.x = (i + 0.5f) * step - 1f; // 放置位置
            point.localPosition = position;// 置入位置
            point.localScale = scale;// 置入大小
            point.SetParent(transform, false); // 將實例化物件放在父物件下
            // false讓對象不在其原始世界位置、旋轉和比例
        }

    }

    void Update()
    {

        for (int i = 0; i < points.Length; i++) 
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.localPosition = position;
        }

    }

    #endregion

}
