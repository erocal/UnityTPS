using System.Threading.Tasks;
using UnityEngine;

public class MapAreaTrigger : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("地圖區域"), Tooltip("這個觸發所屬的地圖區域")]
    [SerializeField] MapAreaType mapArea;

    #endregion

    #region -- 變數參考區 --

    private ActionSystem actionSystem;
    private Organism organism;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;
        organism = Organism.Instance;

    }

    private async Task OnTriggerEnter(Collider other)
    {

        if (other.tag == organism.GetPlayer().tag)
        {

            await actionSystem.MapAreaSwitch((int)mapArea);

        }

    }

    #endregion

}
