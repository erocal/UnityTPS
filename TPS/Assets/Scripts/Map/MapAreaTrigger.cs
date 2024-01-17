using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapAreaTrigger : MonoBehaviour
{
    [Header("地圖區域"), Tooltip("這個觸發所屬的地圖區域")]
    [SerializeField] MapArea mapArea;

    private MapAreaManager mapAreaManager;

    private void Awake()
    {
        if (mapAreaManager == null) mapAreaManager = GetComponentInParent<MapAreaManager>();
    }

    private async Task OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            await mapAreaManager.SwitchMapArea( (int)mapArea );
        }
    }
}
