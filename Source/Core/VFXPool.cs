using MTMTVFX.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Core
{
    public class VFXPool
    {
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        public VFXPool(GameObject prefab, int initialSize = 10, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = Object.Instantiate(prefab, parent);
                obj.SetActive(false);
                EffectAutokill comp = obj.AddComponent<EffectAutokill>();
                comp.pool = this;
                pool.Enqueue(obj);
            }
        }

        public bool TryGet(Vector3 position, Vector3 forward, out GameObject obj)
        {
            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
            }
            else
            {
                obj = null;
                return false;
            }

            obj.transform.position = position;
            obj.transform.forward = forward;
            obj.SetActive(true);
            return true;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}