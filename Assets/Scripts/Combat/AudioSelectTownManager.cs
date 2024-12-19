using UnityEngine;

public class AudioSelectTownManager : MonoBehaviour
{

    #region -- 初始化/運作 --

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            Camera.main.GetComponent<BackgroundMusic>().AudioSelectTown();

        }

    }

    #endregion

}
