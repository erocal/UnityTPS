using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("物件上下移動頻率")]
    [SerializeField] float verticalBobFrequency = 1f;
    [Header("物件上下移動的距離")]
    [SerializeField] float bobbingAmount = 1f;
    [Header("每秒旋轉的角度")]
    [SerializeField] float rotatingSpeed = 360f;

    #endregion

    #region -- 變數參考區 --

    Organism organism;

    private Vector3 startPosition;
    protected GameObject player;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        organism = GameManagerSingleton.Instance.Organism;
        player = organism.PlayerData.Player;

    }

    void Start()
    {

        var rigidbody = GetComponent<Rigidbody>();
        var collider = GetComponent<Collider>();

        rigidbody.isKinematic = true;
        collider.isTrigger = true;

        startPosition = transform.position;

    }

    void Update()
    {

        // 上下移動的公式
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
        transform.position = startPosition + Vector3.up * bobbingAnimationPhase;

        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);

    }

    private void OnTriggerEnter(Collider other)
    {

        if(player == null) player = GameManagerSingleton.Instance.Organism.PlayerData.Player;

        if (player.CompareTag(other.tag))
        {
            PickUpItem();
        }

    }

    #endregion

    #region -- 方法參考區 --

    protected abstract void PickUpItem();

    #endregion

}
