using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField] Image healthImage;
    [SerializeField] Health playerHealth;

    #endregion

    #region -- 變數參考區 --

    Organism organism;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        organism = Organism.Instance;
    }

    void Start()
    {
        if (playerHealth == null) playerHealth = organism.GetPlayer().GetComponent<Health>();
    }

    void Update()
    {
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, playerHealth.GetHealthRatio(), 0.3f);
    }

    #endregion

}
