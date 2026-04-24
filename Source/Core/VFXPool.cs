using BrilliantSkies.PlayerProfiles;
using MTMTVFX.Internal;
using MTMTVFX.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Core
{
    public class VFXPool
    {
        public readonly string modName;
        public readonly string name;
        public readonly Enum type;
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _reserve = new Queue<GameObject>();
        private readonly Queue<GameObject> _rendered = new Queue<GameObject>();
        private int _preferredSize;

        public VFXPool(GameObject prefab, string modName, Enum type, int initialSize = 10, Transform parent = null, bool autokill = true)
        {
            this.modName = modName;
            this.name = prefab.name;
            this.type = type;
            this._prefab = prefab;
            this._parent = parent;
            _preferredSize = initialSize;

            for (int i = 0; i < initialSize; i++) InstantiateNewInReserve(autokill);
        }

        private GameObject InstantiateNewInReserve(bool autokill = true)
        {
            GameObject obj = UnityEngine.Object.Instantiate(_prefab, _parent);

            if (autokill)
            {
                EffectAutokill comp = obj.AddComponent<EffectAutokill>();
                comp.pool = this;
                Utils.AddScript(obj, type, modName);
            }
            else
            {
                EffectManualKill comp = obj.AddComponent<EffectManualKill>();
                comp.pool = this;
                Utils.AddScript(obj, type, modName);
            }

                Return(obj);
            return obj;
        }
        public bool TryGet(Vector3 position, Vector3 forward, out GameObject obj)
        {
            obj = null;

            // no objects in reserve or rendered objects exceed the preferred pool size
            if (_reserve.Count < 1 || _rendered.Count >= _preferredSize)
            {
                // make a new one if the size is dynamic
                if (ProfileManager.Instance.GetModule<SettingsConfig>().ADAPTIVE)
                {
                    obj = InstantiateNewInReserve();
                }
                // otherwise dequeue and use the oldest rendered object
                else
                {
                    obj = _rendered.Dequeue();
                    Return(obj);
                }
            }

            if (_reserve.Count > 0)
            {
                obj = _reserve.Dequeue();
                _rendered.Enqueue(obj);
            }

            obj.transform.position = position;
            obj.transform.forward = forward;
            obj.SetActive(true);
            return true;
        }

        public virtual void Return(GameObject obj)
        {
            obj.SetActive(false);
            _reserve.Enqueue(obj);
        }

        public void OnConfigUpdate()
        {
            if (ProfileManager.Instance.GetModule<SettingsConfig>().ADAPTIVE) return;

            int newSize = Enums.GetCount(type);
            int currentSize = _reserve.Count + _rendered.Count;

            if (newSize > currentSize)
            {
                int temp = newSize - currentSize;
                for (int i = 0; i < temp; ++i)
                {
                    InstantiateNewInReserve();
                }
            }

            _preferredSize = newSize;
        }
    }
}