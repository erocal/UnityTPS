using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuzzleRotateToScreenMiddle : MonoBehaviour
{
    [Header("Ray最大距離")]
    [SerializeField] float maxDistance = 5000f;

    Ray ray;
    RaycastHit hit;

    [SerializeField] float offset_Y = 50f;
    [Header("準星探測距離")]
    [SerializeField] float ultDistance = 40f;

    [HideInInspector] public string info = null;
    private int targetMaskEnemy;
    private int targetMaskTerrain;
    private GameObject player;

    PlayerController playerController;
    gazed ingazed;

    private void Awake()
    {
        ingazed = null;
        targetMaskEnemy = LayerMask.GetMask("Enemy");
        targetMaskTerrain = LayerMask.GetMask("Terrain");
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        
        //ingazed = GetComponent<gazed>();
    }

    // Update is called once per frame
    void Update()
    {
        ingazed = null;
        
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + offset_Y, 0));
        //print("1");
        transform.rotation = Quaternion.LookRotation(ray.GetPoint(maxDistance));
        if (Physics.Raycast(transform.position, ray.GetPoint(maxDistance), out hit, ultDistance, targetMaskEnemy))
        {
            
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * ultDistance, Color.yellow);
            //print("2");
            info = "gazed";
            if (playerController.crosshair.activeInHierarchy != false)
            {
                //print("4");
                ingazed = GameObject.FindGameObjectWithTag("AimImage").GetComponent<gazed>();
                ingazed.Ingazed();
            }
        }
        else
        {
            if (playerController.crosshair.activeInHierarchy != false)
            {
                //print("3");
                ingazed = GameObject.FindGameObjectWithTag("AimImage").GetComponent<gazed>();
                ingazed.NotIngazed();
            }
            info = null;
            Debug.DrawRay(transform.position, ray.GetPoint(maxDistance) * 1000, Color.blue);
        }


        //Debug.DrawLine(transform.position, ray.GetPoint(maxDistance), Color.blue);
    }
}
