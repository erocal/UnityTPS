using UnityEngine;

public abstract class AIController : MonoBehaviour
{
    #region -- �귽�ѦҰ� --

    [Tooltip("�l���Z��")]
    [SerializeField] protected float chaseDistance = 10f;
    [Tooltip("���h�ؼЫ�x�b���ɶ�")]
    [SerializeField] protected float confuseTime = 5f;

    [Header("���޸��|")]
    [SerializeField] protected PatrolPath patrol;
    [Header("�ݭn��FWayPoint���Z��")]
    [SerializeField] protected float waypointToStay = 3f;
    [Header("�ݦbWayPoint���ɶ�")]
    [SerializeField] protected float waypointToWaitTime = 3f;
    [Header("���ޮɪ��t��")]
    [Range(0, 1)]
    [SerializeField] protected float patrolSpeedRatio = 0.5f;

    [Space(20)]
    [Header("�n�P����gameobject�ڸ`�I")]
    [SerializeField] protected GameObject enemyRoot;

    [Space(20)]
    [Header("AI�O�_����"), ReadOnly]
    [SerializeField] protected bool isBeHit;

    [Header("���a")]
    [SerializeField] protected GameObject player;
    [Header("�ʵe���")]
    [SerializeField] protected Animator animator;
    [Header("Health")]
    [SerializeField] protected Health health;
    [Header("�I����")]
    [SerializeField] protected Collider collider;

    #endregion

    #region -- �ܼưѦҰ� --

    [Tooltip("�W���ݨ쪱�a���ɶ�"), ReadOnly]
    [SerializeField] protected float sinceLastSawPlayerTimer = Mathf.Infinity;
    [Tooltip("��l�ͦ��y��"), ReadOnly]
    [SerializeField] protected Vector3 aiSpawnPostion;
    [Tooltip("��e�ݭn��F��WayPoint�s��"), ReadOnly]
    [SerializeField] protected int currentWaypointIndex = 0;
    [Tooltip("�Z���W����FWayPoint���ɶ�"), ReadOnly]
    [SerializeField] protected float sinceArriveWayPointTimer = 0;

    #endregion

    #region -- ��l��/�B�@ --

    private void Update()
    {
        if (health.IsDead()) return;
    }

    #endregion

    #region -- ��k�ѦҰ� --

    /// <summary>
    /// �]�wAI��l�ͦ���m
    /// </summary>
    /// <param name="spawnPostion">��l�ͦ���m</param>
    protected void SetSpawnPosition(Vector3 spawnPostion)
    {
        this.aiSpawnPostion = spawnPostion;
    }

    /// <summary>
    /// AI�үవ�X���欰
    /// </summary>
    protected abstract void AIBehavior();

    /// <summary>
    /// �B�zAI�����欰
    /// </summary>
    protected abstract void AttackBehavior();

    // ���ަ欰
    protected abstract void PatrolBehavior();

    // �x�b���ʧ@�欰
    protected abstract void ConfuseBehavior();

    /// <summary>
    /// �ݨ����a
    /// </summary>
    protected void SawPlayer()
    {
        sinceLastSawPlayerTimer = 0;
    }

    // �ˬd�O�_�w�g��FWayPoint
    protected bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWaypointIndex)) < waypointToStay);
    }

    /// <summary>
    /// �O�_�p��l���Z����
    /// </summary>
    protected bool IsRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    /// <summary>
    /// �T�{�O�_����
    /// </summary>
    protected bool CheckIsBeHit()
    {
        return isBeHit;
    }

    protected void UpdateLastSawPlayerTimer()
    {
        sinceLastSawPlayerTimer += Time.deltaTime;
    }

    protected void UpdateArriveWayPointTimer()
    {
        sinceArriveWayPointTimer += Time.deltaTime;
    }

    #region -- �ƥ���� --

    /// <summary>
    /// AI��������ɳB�z��k
    /// </summary>
    protected void OnDamage()
    {
        //  ��������ɡAĲ�o���ƥ�
        isBeHit = true;
    }

    /// <summary>
    /// AI���`�ɳB�z��k
    /// </summary>
    protected abstract void OnDie();

    #endregion

    #region -- DrawGizmo --

    // Called by Unity
    // �o�O�ۦ�ø�svisable�i���ƪ���A�Ψӳ]�p�Ǫ��l�ܪ��a���d��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    #endregion

    #endregion
}
