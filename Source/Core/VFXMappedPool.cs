using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MTMTVFX.Core
{
    public class VFXMappedPool<T>
    {
        private readonly VFXPool _pool;
        public readonly Dictionary<T, GameObject> _mapping;

        public VFXMappedPool(GameObject prefab, string modName, Enum type, int initialSize = 10, Transform parent = null)
        {
            _pool = new VFXPool(prefab, modName, type, initialSize, parent);
            _mapping = new Dictionary<T, GameObject>();
        }

        public bool TryGet(T key, Vector3 position, Vector3 forward, out GameObject obj)
        {
            if (_mapping.TryGetValue(key, out obj))
            {
                return true;
            }

            if (_pool.TryGet(position, forward, out obj))
            {
                _mapping[key] = obj;
                return true;
            }

            return false;
        }
    }
}
