using UnityEngine;

namespace ToolBox.Pools
{
	public static class PoolHelper
	{
		public static void Populate(this GameObject prefab, int count)
		{
			Pool.GetPoolByPrefab(prefab).Populate(count);
		}

        public static void Populate(this GameObject prefab, Transform transform, int count)
        {
            Pool.GetPoolByPrefab(prefab).Populate(transform, count);
        }

        public static void Clear(this GameObject prefab, bool destroyActive)
		{
			Pool.GetPoolByPrefab(prefab, false)?.Clear(destroyActive);
		}

		public static GameObject Reuse(this GameObject prefab)
		{
			return Pool.GetPoolByPrefab(prefab).Reuse();
		}

		public static GameObject Reuse(this GameObject prefab, Transform parent)
		{
			return Pool.GetPoolByPrefab(prefab).Reuse(parent);
		}

		public static GameObject Reuse(this GameObject prefab, Transform parent, bool worldPositionStays)
		{
			return Pool.GetPoolByPrefab(prefab).Reuse(parent, worldPositionStays);
		}

		public static GameObject Reuse(this GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return Pool.GetPoolByPrefab(prefab).Reuse(position, rotation);
		}

		public static GameObject Reuse(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			return Pool.GetPoolByPrefab(prefab).Reuse(position, rotation, parent);
		}

		public static T Reuse<T>(this GameObject prefab) where T : Component
		{
			return prefab.Reuse().GetComponent<T>();
		}

		public static T Reuse<T>(this GameObject prefab, Transform parent) where T : Component
		{
			return prefab.Reuse(parent).GetComponent<T>();
		}

		public static T Reuse<T>(this GameObject prefab, Transform parent, bool worldPositionStays) where T : Component
		{
			return prefab.Reuse(parent, worldPositionStays).GetComponent<T>();
		}

		public static T Reuse<T>(this GameObject prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			return prefab.Reuse(position, rotation).GetComponent<T>();
		}

		public static T Reuse<T>(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
		{
			return prefab.Reuse(position, rotation, parent).GetComponent<T>();
		}

		public static void Release(this GameObject instance)
		{
			var isPooled = Pool.GetPoolByInstance(instance, out var pool);

			if (isPooled)
			{
				pool.Release(instance);
			}
			else
			{
				Object.Destroy(instance);
			}
		}

        public static void Release(this GameObject instance, Transform parent)
        {
            var isPooled = Pool.GetPoolByInstance(instance, out var pool);

            if (isPooled)
            {
                pool.Release(instance, parent);
            }
            else
            {
                Object.Destroy(instance);
            }
        }

    }
}
