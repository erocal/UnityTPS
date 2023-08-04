using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManger : MonoBehaviour
{
    [Header("要生成的Enemy物件")]
    [SerializeField] GameObject enemy;
    [Header("生成的間隔時間")]
    [SerializeField] float spawnTime = 3f;
    [Header("需要生成的Enemy數量")]
    [SerializeField] int spawnAmount = 10;
    [SerializeField] Transform[] spawnPoint;

    bool hasbeentrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasbeentrigger) return;

        if (other.gameObject.tag == "Player")
        {
            hasbeentrigger = true;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            // 隨機生成長度
            int spawnPointIndex = Random.Range(0, spawnPoint.Length);

            Instantiate(enemy, spawnPoint[spawnPointIndex].position, spawnPoint[spawnPointIndex].rotation);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
