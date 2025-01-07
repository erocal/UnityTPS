using ToolBox.Pools;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolContainerSO", menuName = "Scriptable Objects/PoolContainerSO")]
public class PoolContainerSO : ScriptableObject
{

    [SerializeField] public PoolContainer[] _pools;

    [System.Serializable]
    public struct PoolContainer
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
