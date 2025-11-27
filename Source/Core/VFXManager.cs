using BrilliantSkies.Core.Logger;
using MTMTVFX.Internal;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace MTMTVFX.Core
{
    public class VFXManager
    {
        public static VFXManager Instance { get; private set; } = new VFXManager();

        private Queue<GameObject> _rendered = new Queue<GameObject>();
        private GameObject _vfxRoot;

        private VFXManager()
        {
            _vfxRoot = new GameObject("MTMTVFX_Root");
            VFXRegistry.Init();
        }

        private void AddToQueue(GameObject obj)
        {
            _rendered.Enqueue(obj);

            while (_rendered.Count > Configurables.maxVFX)
            {
                GameObject old = _rendered.Dequeue();
                if (old != null)
                {
                    Object.Destroy(old);
                }
            }
        }

        /// <summary>
        /// Create VFX by name, requires at least one live particle
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public GameObject Create(string name, Vector3 pos, Vector3 forward)
        {
            GameObject prefab;
            bool flag = VFXRegistry.TryGetEffect(name, out prefab);
            if (flag)
            {
                GameObject obj = Object.Instantiate<GameObject>(prefab, _vfxRoot.transform);
                obj.transform.position = pos;
                obj.transform.forward = forward;
                obj.AddComponent<EffectAutokill>();

                AddToQueue(obj);

                Util.LogInfo<VFXManager>($"Effect {name} created!");

                string o = "";
                foreach (GameObject go in _rendered)
                {
                    o += go.name;
                    o += '\n';
                }
                Util.LogInfo<VFXManager>("Current rendered effects:\n" + o);
                return obj;
            }
            else
            {
                Util.LogError<VFXManager>($"Effect {name} not found!");
                return null;
            }

        }
    }
}
