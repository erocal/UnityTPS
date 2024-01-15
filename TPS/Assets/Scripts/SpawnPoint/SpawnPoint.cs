using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("地圖區域"), Tooltip("這個重生點所屬的地圖區域")]
    [SerializeField] MapArea mapArea;

    [Header("重生點特效"), Tooltip("觸發重生點時提示玩家的特效")]
    [SerializeField] GameObject spawnPointParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();

            playerController.spawn = this.transform.position;// 更新重生點
            playerController.playerStandMapArea = mapArea;

            // 觸發特效
            var spawnPointParticle = Instantiate(this.spawnPointParticle, this.transform);
            spawnPointParticle.SetActive(true);
        }
    }
}
