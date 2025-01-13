using UnityEngine;

public class PatrolPath : MonoBehaviour
{

    #region -- 資源參考區 --

    // WayPoint的半徑
    [Tooltip("WayPoint的半徑")]
    [SerializeField] float wayPointGizmosRadius = 1f;

    #endregion

    #region -- 初始化/運作 --

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.green;
            int j = GetNextWayPointNumber(i);
            Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            Gizmos.DrawSphere(GetWayPointPosition(i), wayPointGizmosRadius);
        }
    }

    #endregion

    #region -- 方法參考區 --

    // 取得下一個WayPoint的編號
    public int GetNextWayPointNumber(int wayPointNumber)
    {
        if (wayPointNumber + 1 > transform.childCount - 1)
        {
            return 0;
        }
        return wayPointNumber + 1;
    }

    // 取得WayPoint的位置
    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber).position;
    }

    #endregion

}
