using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{

    #region -- 資源參考區 --

    [Tooltip("跟隨的Player目標")]
    [SerializeField] Transform player;
    [Tooltip("跟目標的最大距離")]
    [SerializeField] float distanceToTarget;
    [Tooltip("起始高度")]
    [SerializeField] float startHeight;
    [Tooltip("平滑的移動時間")]
    [SerializeField] float smoothTime;
    [Tooltip("滾輪軸靈敏度")]
    [SerializeField] float sensitivityOffset_z;
    [Tooltip("最小垂直 Y offset")]
    [SerializeField] float minOffset_Y;
    [Tooltip("最大垂直 Y offset")]
    [SerializeField] float maxOffset_Y;

    #endregion

    #region -- 變數參考區 --

    float offset_Y;

    InputController input;
    Vector3 smoothPosition = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    #endregion

    #region -- 初始化/運作 --

    private void Awake() 
    {
        input = GameManagerSingleton.Instance.InputController;
        transform.position = player.position + Vector3.up * startHeight;  
        offset_Y = startHeight;
    }

    private void LateUpdate()
    {
        if (input.GetMouseScrollWheelAxis() != 0)
        {
            offset_Y += input.GetMouseScrollWheelAxis() * sensitivityOffset_z;
            offset_Y = Mathf.Clamp(offset_Y, minOffset_Y, maxOffset_Y);
            Vector3 offsetTarget = player.position + player.up * offset_Y;
            transform.position = Vector3.Lerp(transform.position, offsetTarget, smoothTime);
        }
        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * offset_Y, ref currentVelocity, smoothTime);
        }
        transform.position = smoothPosition;
    }

    #endregion

    #region -- 方法參考區 --

    // 檢查與目標的距離
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }

    #endregion

}
