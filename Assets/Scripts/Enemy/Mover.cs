using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    #region -- 資源參考區 --

    [Tooltip("最大移動速度")]
    [SerializeField] float maxSpeed = 6f;
    [Tooltip("改變動畫速度")]
    [SerializeField] float animatorChangeRatio = 0.2f;

    #endregion

    #region -- 變數參考區 --

    NavMeshAgent navMeshAgent;
    float nextSpeed;

    // 上一幀的移動速度
    float lastFrameSpeed;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    #endregion

    #region -- 方法參考區 --

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        // 將全局NavMesh速度變量，轉換成local的速度變量。
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);

        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / maxSpeed);
    }

    /// <summary>
    /// 透過NavMeshAgent以可變速度朝目標前進
    /// </summary>
    /// <param name="destination">目標的位置</param>
    /// <param name="speedRatio">移動速率(0~1)</param>
    public void MoveTo(Vector3 destination, float speedRatio)
    {

        if (!navMeshAgent.isOnNavMesh) NavMeshHelper.CantFindNavMesh(gameObject);

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;

    }

    public void CancelMove()
    {

        // 停止導航系統
        navMeshAgent.isStopped = true;

    }

    #endregion

}
