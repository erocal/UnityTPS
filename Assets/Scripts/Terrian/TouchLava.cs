using UnityEngine;

public class TouchLava : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("岩漿傷害")]
    [SerializeField] private int lavaDamage = 15;

    #endregion

    #region -- 初始化/運作 --

    private void OnTriggerEnter(Collider other)
    {

        var targetHealth = other.GetComponent<Health>();

        if (targetHealth)
            targetHealth.TakeDamage(lavaDamage);

    }

    #endregion

}
