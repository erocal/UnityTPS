using UnityEngine;

public class DamageObject : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("傷害")]
    [SerializeField] private int dmage = 15;

    #endregion

    #region -- 初始化/運作 --

    private void OnTriggerEnter(Collider other)
    {

        var targetHealth = other.GetComponent<Health>();

        if (targetHealth)
            targetHealth.TakeDamage(dmage);

    }

    #endregion

}
