using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("�a�ϰϰ�"), Tooltip("�o�ӭ����I���ݪ��a�ϰϰ�")]
    [SerializeField] MapArea mapArea;

    [Header("�����I�S��"), Tooltip("Ĳ�o�����I�ɴ��ܪ��a���S��")]
    [SerializeField] GameObject spawnPointParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();

            playerController.spawn = this.transform.position;// ��s�����I
            playerController.playerStandMapArea = mapArea;

            // Ĳ�o�S��
            var spawnPointParticle = Instantiate(this.spawnPointParticle, this.transform);
            spawnPointParticle.SetActive(true);
        }
    }
}
