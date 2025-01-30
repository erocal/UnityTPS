using UnityEngine;

public class PlagueDoctorMover : MonoBehaviour
{

    #region -- 資源參考區 --

    [Tooltip("最大移動速度")]
    [SerializeField] float maxSpeed = 6f;
    [Tooltip("改變動畫速度")]
    [SerializeField] float animatorChangeRatio = 0.2f;

    #endregion

    #region -- 變數參考區 --

    Organism organism;

    // 上一幀的移動速度
    float lastFrameSpeed;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        organism = GameManagerSingleton.Instance.Organism;

    }

    private void Update()
    {

        UpdateAnimator();

    }

    #endregion

    #region -- 方法參考區 --

    private void UpdateAnimator()
    {

        Vector3 velocity = organism.PlagueDoctorData.BossNavMeshAgent.velocity;
        // 將全局NavMesh速度變量，轉換成local的速度變量。
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);

        organism.PlagueDoctorData.BossAnimator.SetFloat("WalkSpeed", lastFrameSpeed / maxSpeed);

    }

    public void MoveTo(Vector3 destination, float speedRatio)
    {

        var navMeshAgent = organism.PlagueDoctorData.BossNavMeshAgent;

        if (!navMeshAgent.isOnNavMesh) NavMeshHelper.CantFindNavMesh(gameObject);

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;

    }

    /// <summary>
    /// 停止NavMeshAgent導航系統
    /// </summary>
    public void CancelMove()
    {

        // 停止導航系統
        organism.PlagueDoctorData.BossNavMeshAgent.isStopped = true;

    }

    #endregion

}
