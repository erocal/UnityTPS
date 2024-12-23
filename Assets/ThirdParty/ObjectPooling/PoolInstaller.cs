using UnityEngine;

namespace ToolBox.Pools
{
    [DefaultExecutionOrder(-9999), DisallowMultipleComponent]
    public sealed class PoolInstaller : MonoBehaviour
    {

        #region -- �귽�ѦҰ� --

        [SerializeField] private PoolContainerSO poolContainerSO;

        #endregion

        #region -- �ܼưѦҰ� --

        #region -- �`�� --

        private const string SO_ADDRESS = "poolcontainerso";

        #endregion

        #endregion

        #region -- ��l��/�B�@ --

        private async void Awake()
        {

            if (poolContainerSO == null)
            {

                var handle = await AddrssableAsync.LoadAsync<PoolContainerSO>(SO_ADDRESS);

                if (handle.IsValid())
                {
                    poolContainerSO = handle.Result;
                }

            }

            var _pools = poolContainerSO._pools;


            for (var i = 0; i < _pools.Length; i++)
                _pools[i].Populate(this.transform);

        }

        #endregion

        [System.Serializable]
        private struct PoolContainer
        {
            [SerializeField] private GameObject _prefab;
            [SerializeField, Min(1)] private int _startCount;

            public void Populate()
            {
                _prefab.Populate(_startCount);
            }

            public void Populate(Transform transform)
            {
                _prefab.Populate(transform, _startCount);
            }

        }
    }
}
