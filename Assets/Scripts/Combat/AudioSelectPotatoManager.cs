using UnityEngine;

public class AudioSelectPotatoManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            
            Camera.main.GetComponent<BackgroundMusic>().AudioSelectPotato();

        }

    }
}
