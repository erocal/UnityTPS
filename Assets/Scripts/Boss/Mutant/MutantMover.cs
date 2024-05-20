using UnityEngine;
using UnityEngine.AI;

public class MutantMover : MonoBehaviour
{
    [Header("旋轉速度")]
    [SerializeField] float rotateSpeed = 3f;

    NavMeshAgent navmeshAgent;
    Animator animator;

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
}
