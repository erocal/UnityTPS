using UnityEngine;

public class AudioSelectWindManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            
            Camera.main.GetComponent<BackgroundMusic>().AudioSelectWind();

        }

    }
}
