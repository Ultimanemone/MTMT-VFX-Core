using BrilliantSkies.Core.Logger;
using MTMTVFX.Internal;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MTMTVFX.Core
{
    internal class VFXManager
    {
        public static VFXManager Instance { get; private set; } = new VFXManager();

        public IReadOnlyDictionary<string, GameObject> VFXAssets => _VFXAssets;
        private Dictionary<string, GameObject> _VFXAssets = new Dictionary<string, GameObject>();
        private Queue<GameObject> _rendered = new Queue<GameObject>();
        private GameObject _vfxRoot;

        private VFXManager()
        {
            _vfxRoot = new GameObject("MTMTVFX_Root");
            _VFXAssets = CorePlugin.GetAllAssets();
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
            bool flag = _VFXAssets.TryGetValue(name, out prefab);
            if (flag)
            {
                GameObject obj = Object.Instantiate<GameObject>(prefab, _vfxRoot.transform);
                obj.transform.position = pos;
                obj.transform.forward = forward;
                obj.AddComponent<EffectAutokill>();
                Util.LogInfo<VFXManager>($"MTMTVFX : Effect {name} created!");
                return obj;
            }
            else
            {
                Util.LogError<VFXManager>($"MTMTVFX : Effect {name} not found!");
                return null;
            }
        }
    }
}
