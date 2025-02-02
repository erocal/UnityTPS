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

    private ActionSystem actionSystem;

    private Ray ray;
    private RaycastHit hit;

    [HideInInspector] public string info = null;
    private int targetMaskEnemy;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

        targetMaskEnemy = LayerMask.GetMask("Enemy");
        
    }

    void Update()
    {

        var crosshair = GameManagerSingleton.Instance.UISystem.CrossHair;
        
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + offset_Y, 0));
        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        if (Physics.Raycast(transform.position, ray.GetPoint(maxDistance), out hit, ultDistance, targetMaskEnemy))
        {
            
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * ultDistance, Color.yellow);
            info = "Gazed";
            if (crosshair.activeInHierarchy != false)
                actionSystem.Gazed(true);

        }
        else
        {
            if (crosshair.activeInHierarchy != false)
                actionSystem.Gazed(false);

            info = null;
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * 1000, Color.blue);
        }

        //Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);

    }

    #endregion

}
