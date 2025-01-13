using System.Collections;
using UnityEngine;

public class EnemySpawnManger : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("要生成的Enemy物件")]
    [SerializeField] GameObject enemy;
    [Header("生成的間隔時間")]
    [SerializeField] float spawnTime = 3f;
    [Header("需要生成的Enemy數量")]
    [SerializeField] int spawnAmount = 10;
    [SerializeField] Transform[] spawnPoint;

    #endregion

    #region -- 變數參考區 --

    bool hasbeentrigger;

    #endregion

    #region -- 初始化/運作 --

    private void OnTriggerEnter(Collider other)
    {

        if (hasbeentrigger) return;

        if (other.tag == "Player")
        {
            hasbeentrigger = true;
            StartCoroutine(Spawn());
        }

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 敵人的生成
    /// </summary>
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

    #endregion

}
