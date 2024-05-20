using UnityEngine;

public class MuzzleRotateToScreenMiddle : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("Ray最大距離")]
    [SerializeField] float maxDistance = 5000f;
    
    [SerializeField] float offset_Y = 50f;
    [Header("準星探測距離")]
    [SerializeField] float ultDistance = 40f;

    #endregion

    #region -- 變數參考區 --

    Organism organism;

    Ray ray;
    RaycastHit hit;

    [HideInInspector] public string info = null;
    private int targetMaskEnemy;
    private GameObject player;

    PlayerController playerController;
    gazed ingazed;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        organism = Organism.Instance;

        ingazed = null;
        targetMaskEnemy = LayerMask.GetMask("Enemy");
        player = organism.GetPlayer();
        playerController = player.GetComponent<PlayerController>();
        
        //ingazed = GetComponent<gazed>();
    }

    void Update()
    {
        ingazed = null;
        
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + offset_Y, 0));
        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        if (Physics.Raycast(transform.position, ray.GetPoint(maxDistance), out hit, ultDistance, targetMaskEnemy))
        {
            
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * ultDistance, Color.yellow);
            info = "gazed";
            if (playerController.GetCrosshair().activeInHierarchy != false)
            {
                ingazed = GameObject.FindGameObjectWithTag("AimImage").GetComponent<gazed>();
                ingazed.Ingazed();
            }
        }
        else
        {
            if (playerController.GetCrosshair().activeInHierarchy != false)
            {
                ingazed = GameObject.FindGameObjectWithTag("AimImage").GetComponent<gazed>();
                ingazed.NotIngazed();
            }
            info = null;
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * 1000, Color.blue);
        }


        //Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);
    }

    #endregion

}
