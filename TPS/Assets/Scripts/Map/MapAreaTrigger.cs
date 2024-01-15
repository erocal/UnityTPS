using System.Collections.Generic;
using UnityEngine;

public class MapAreaTrigger : MonoBehaviour
{
    [Header("�a�ϰϰ�"), Tooltip("�o��Ĳ�o���ݪ��a�ϰϰ�")]
    [SerializeField] MapArea mapArea;

    private MapAreaManager mapAreaManager;

    private void Awake()
    {
        if (mapAreaManager == null) mapAreaManager = GetComponentInParent<MapAreaManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mapAreaManager.SwitchMapArea( (int)mapArea );
        }
    }
}
