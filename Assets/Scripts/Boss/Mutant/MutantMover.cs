using UnityEngine;
using UnityEngine.AI;

public class MutantMover : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("旋轉速度")]
    [SerializeField] float rotateSpeed = 3f;

    #endregion

    #region -- 變數參考區 --

    NavMeshAgent navmeshAgent;
    Animator animator;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navmeshAgent.isStopped = true;
        navmeshAgent.updateRotation = false;
    }

    private void Update()
    {
        if (!navmeshAgent.isStopped)
        {
            // 避開障礙物
            Vector3 targetPosition = navmeshAgent.steeringTarget - transform.position;

            if (targetPosition == Vector3.zero) return;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition, Vector3.up), rotateSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region -- 方法參考區 --

    public void MoveTo(Vector3 destination)
    {
        navmeshAgent.isStopped = false;
        navmeshAgent.destination = destination;
        animator.SetBool("Move", true);
    }

    public void CancelMove()
    {
        // 停止導航系統
        navmeshAgent.isStopped = true;
        animator.SetBool("Move", false);
    }

    #endregion

}
