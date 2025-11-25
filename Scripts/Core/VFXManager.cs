using BrilliantSkies.Core.Logger;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MTMTVFX.Core
{
    internal class VFXManager
    {
        public static VFXManager Instance = new VFXManager();

        public IReadOnlyDictionary<string, GameObject> VFX => _VFX;
        private Dictionary<string, GameObject> _VFX = new Dictionary<string, GameObject>();
        private Queue<GameObject> _rendered = new Queue<GameObject>();

        private VFXManager()
        {
            _VFX = CorePlugin.GetAllAssets();
        }

        private void AddToQueue(GameObject obj)
        {
            _rendered.Enqueue(obj);

            bool flag = _rendered.Count > Configurables.maxVFX;
            while (flag)
            {
                if (_rendered.Peek() == null) _rendered.Dequeue();
            }
        }

        /// <summary>
        /// Create VFX by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public GameObject Create(string name, Vector3 pos, Vector3 forward)
        {
            GameObject prefab;
            bool flag = _VFX.TryGetValue(name, out prefab);
            if (flag)
            {
                GameObject obj = Object.Instantiate<GameObject>(prefab);
                obj.transform.position = pos;
                obj.transform.forward = forward;
                return obj;
            }
            else
            {
                AdvLogger.LogError($"MTMTVFX : Effect {name} not found!", LogOptions.Popup);
                return null;
            }
        }
    }
}
