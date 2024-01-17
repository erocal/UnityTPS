using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    [Header("物件存放處"), Tooltip("放入系統預備生成使用的子彈")]
    [SerializeField] List<GameObject> projectile = new List<GameObject>();

    [Header("生成數量"), Tooltip("系統預備生成的子彈數量")]
    [SerializeField] int initailSize = 20;

    private Dictionary<int, Queue<GameObject>> m_pool = new Dictionary<int, Queue<GameObject>>();

    public const string PROJECTILEPOOLNAME = "ProjectilePool";

    void Awake()
    {
        // 重新命名為子彈物件池名字
        this.gameObject.name = PROJECTILEPOOLNAME;

        if (projectile != null)
        {
            for (int i = 0; i < projectile.Count; i++)
            {
                m_pool[i] = new Queue<GameObject>();

                for (int cnt = 0; cnt < initailSize; cnt++)
                {
                    GameObject go = Instantiate(projectile[i], this.transform) as GameObject;
                    m_pool[i].Enqueue(go); 
                    go.SetActive(false);
                }
            }
            
        }
        
        DontDestroyOnLoad(gameObject);

    }

    public Projectile ReUse(ProjectileId projectileId, Vector3 position, Quaternion rotation)
    {

        if (m_pool.Count > 0)
        {
            GameObject reuse = m_pool[(int)projectileId].Dequeue();
            reuse.transform.position = position;
            reuse.transform.rotation = rotation;
            reuse.SetActive(true);

            return reuse.GetComponent<Projectile>();
        }
        else
        {
            GameObject go = Instantiate(projectile[(int)projectileId]) as GameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;

            return go.GetComponent<Projectile>();
        }

    }


    public void Recovery(ProjectileId projectileId, GameObject recovery)
    {
        recovery.SetActive(false);
        m_pool[(int)projectileId].Enqueue(recovery);
    }
}