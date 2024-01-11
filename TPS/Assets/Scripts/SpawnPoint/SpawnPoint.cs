using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("重生點特效"), Tooltip("觸發重生點時提示玩家的特效")]
    [SerializeField] GameObject spawnPointParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();

            playerController.spawn = this.transform.position;// 更新重生點

            // 觸發特效
            var spawnPointParticle = Instantiate(this.spawnPointParticle, this.transform);
            spawnPointParticle.SetActive(true);
        }
    }
}
