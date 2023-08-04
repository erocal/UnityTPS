using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSelectPotatoManager : MonoBehaviour
{
    [Header("相機")]
    [SerializeField] GameObject camera;

    bool hasbeentrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasbeentrigger) return;

        if (other.gameObject.tag == "Player")
        {
            //hasbeentrigger = true;
            camera.GetComponent<ThirdPersonCamera>().AudioSelectPotato();
        }
    }
}
