using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSelectSadManager : MonoBehaviour
{
    [Header("相機")]
    [SerializeField] GameObject camera;

    bool hasbeentrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasbeentrigger) return;

        if (other.tag == "Player")
        {
            //hasbeentrigger = true;
            camera.GetComponent<BackgroundMusic>().AudioSelectSad();
        }
    }
}
