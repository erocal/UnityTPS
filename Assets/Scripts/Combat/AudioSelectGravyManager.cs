using UnityEngine;

public class AudioSelectGravyManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            
            Camera.main.GetComponent<BackgroundMusic>().AudioSelectGravy();

        }

    }
}
