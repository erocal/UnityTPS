using UnityEngine;

public class PlagueDoctorMover : MonoBehaviour
{
    [Tooltip("最大移動速度")]
    [SerializeField] float maxSpeed = 6f;
    [Tooltip("改變動畫速度")]
    [SerializeField] float animatorChangeRatio = 0.2f;

    UnityEngine.AI.NavMeshAgent navMeshAgent;
    float nextSpeed;

    // 上一幀的移動速度
    float lastFrameSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        // 將全局NavMesh速度變量，轉換成local的速度變量。
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);

        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed / maxSpeed);
    }

    public void MoveTo(Vector3 destination, float speedRatio)
    {
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
        navMeshAgent.isStopped = true;
    }
    
}
