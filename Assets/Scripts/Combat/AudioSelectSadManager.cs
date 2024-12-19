using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSelectSadManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            
            Camera.main.GetComponent<BackgroundMusic>().AudioSelectSad();

        }

    }
}
