using MTMTVFX.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Core
{
    public class VFXPool
    {
        public readonly string modName;
        public readonly Enum type;
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly Queue<GameObject> pool = new Queue<GameObject>();
        private readonly Queue<GameObject> rendered = new Queue<GameObject>();

        public VFXPool(GameObject prefab, string modName, Enum type, int initialSize = 10, Transform parent = null)
        {
            this.modName = modName;
            this.type = type;
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = UnityEngine.Object.Instantiate(prefab, parent);
                obj.SetActive(false);

                EffectAutokill comp = obj.AddComponent<EffectAutokill>();
                comp.pool = this;
                Util.AddScript(obj, type, modName);

                pool.Enqueue(obj);
            }
        }

        public bool TryGet(Vector3 position, Vector3 forward, out GameObject obj)
        {
            obj = null;
            if (pool.Count < 1)
            {
                obj = rendered.Dequeue();
                Return(obj);
            }

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
                rendered.Enqueue(obj);
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