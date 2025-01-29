using UnityEngine;

[RequireComponent(typeof(MutantAIController))]
public class MutantMover : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("旋轉速度")]
    [SerializeField] float rotateSpeed = 3f;

    #endregion

    #region -- 變數參考區 --

    Organism organism;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        organism = Organism.Instance;

        var navMeshAgent = organism.MutantData.MutantNavMeshAgent;
        navMeshAgent.isStopped = true;
        navMeshAgent.updateRotation = false;

    }

    private void Update()
    {

        var navMeshAgent = organism.MutantData.MutantNavMeshAgent;

        if (!navMeshAgent.isStopped)
        {
            // 避開障礙物
            Vector3 targetPosition = navMeshAgent.steeringTarget - transform.position;

            if (targetPosition == Vector3.zero) return;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPosition, Vector3.up), rotateSpeed * Time.deltaTime);
        }

    }

    #endregion

    #region -- 方法參考區 --

    public void MoveTo(Vector3 destination)
    {

        var navMeshAgent = organism.MutantData.MutantNavMeshAgent;

        if (!navMeshAgent.isOnNavMesh) NavMeshHelper.CantFindNavMesh(gameObject);

        navMeshAgent.isStopped = false;
        navMeshAgent.destination = destination;
        organism.MutantData.MutantAnimator.SetBool("Move", true);

    }

    public void CancelMove()
    {

        // 停止導航系統
        organism.MutantData.MutantNavMeshAgent.isStopped = true;
        organism.MutantData.MutantAnimator.SetBool("Move", false);

    }

    #endregion

}
