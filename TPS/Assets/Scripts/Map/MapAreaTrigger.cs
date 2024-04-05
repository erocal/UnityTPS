using System.Threading.Tasks;
using UnityEngine;

public class MapAreaTrigger : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("地圖區域"), Tooltip("這個觸發所屬的地圖區域")]
    [SerializeField] MapArea mapArea;

    #endregion

    #region -- 變數參考區 --

    private MapAreaManager mapAreaManager;

    #endregion

    #region -- 初始化/運作 --

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

    #endregion

}
