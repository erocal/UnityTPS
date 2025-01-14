using UnityEngine;
using UnityEngine.UI;

public class Gazed : MonoBehaviour
{

    #region -- 方法參考區 --

    public void Ingazed()
    {

        this.GetComponent<Image>().color = new Color32(0, 255, 0, 255);

    }

    public void NotIngazed()
    {

        this.GetComponent<Image>().color = new Color32(229, 23, 24, 168);

    }

    #endregion

}
