using System.Threading.Tasks;
using UnityEngine;

public static class NavMeshHelper
{
    
    public static async void CantFindNavMesh(GameObject gameObject)
    {

        gameObject.SetActive(false);

        await Task.Delay(5000);

        gameObject.SetActive(true);

    }

}
