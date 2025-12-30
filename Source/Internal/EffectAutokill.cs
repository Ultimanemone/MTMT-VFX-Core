using MTMTVFX.Core;
using UnityEngine;


namespace MTMTVFX.Internal
{
    /// <summary>
    /// Script to kill vfx objects when they run out of the maximum lifetime, or are no longer rendering particles
    /// </summary>
    public class EffectAutokill : MonoBehaviour
    {
        public ParticleSystem[] psList { get; private set; }
        public VFXPool pool;
        private const float maxLifetime = 12f;

        private void Awake()
        {
            psList = GetComponentsInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            GetComponent<ParticleSystem>().Play(true);
            if (GetComponentInChildren<LineRenderer>() != null)
            {
                Debug.Log("flag");
            }
        }

        private void LateUpdate()
        {
            if (GetComponentInChildren<LineRenderer>() != null && gameObject.activeSelf)
            {
                Debug.Log("flag");
            }

            bool flag = (psList[0] == null || maxLifetime < psList[0].time) && psList[0].time > Time.deltaTime;
            if (flag)
            {
                pool.Return(gameObject);
            }

            foreach (ParticleSystem ps in psList)
            {
                if (ps.particleCount > 0 || ps.isEmitting) goto B1;
            }

            Core.Util.LogInfo<EffectAutokill>($"Effect {gameObject.name} killed");
            pool.Return(gameObject);

        B1:
            return;
        }
    }
}
