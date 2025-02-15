using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class InteractiveParticlesEffectActive : MonoBehaviour
{

    #region -- 初始化/運作 --

    private void Awake()
    {

        GameManagerSingleton.Instance.ActionSystem.OnLoginCameraMove += () =>
        {

            this.gameObject.SetActive(true);

        };

        this.gameObject.SetActive(false);

    }

    #endregion

}
