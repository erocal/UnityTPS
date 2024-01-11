using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("�����I�S��"), Tooltip("Ĳ�o�����I�ɴ��ܪ��a���S��")]
    [SerializeField] GameObject spawnPointParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();

            playerController.spawn = this.transform.position;// ��s�����I

            // Ĳ�o�S��
            var spawnPointParticle = Instantiate(this.spawnPointParticle, this.transform);
            spawnPointParticle.SetActive(true);
        }
    }
}
