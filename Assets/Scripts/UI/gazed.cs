using UnityEngine;
using UnityEngine.UI;

public class Gazed : MonoBehaviour
{
    private GameObject muzzle;
    private GameObject player;
    // Start is called before the first frame update
    /*private void Start()
    {
        muzzle = GameObject.FindGameObjectWithTag("Muzzle");
    }*/
    
    
    public void Ingazed()
    {
        /*player = GameObject.FindGameObjectWithTag("Player");
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager)
        {
            if (weaponManager.GetActiveWeapon() != null)
            {
                this.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            }
        }*/
        this.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
    }

    public void NotIngazed()
    {
        this.GetComponent<Image>().color = new Color32(229, 23, 24, 168);
    }
}
