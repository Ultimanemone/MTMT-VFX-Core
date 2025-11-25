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
        private const float maxLifetime = 12f;

        private void Awake()
        {
            psList = GetComponentsInChildren<ParticleSystem>();
        }

        private void LateUpdate()
        {
            bool flag = psList[0] == null || maxLifetime < psList[0].time;
            if (flag)
            {
                Destroy(this);
            }

            foreach (ParticleSystem ps in psList)
            {
                bool flag1 = ps.particleCount > 0;
                if (flag1) goto B1;

                bool flag2 = ps.subEmitters.enabled;
                if (flag2)
                {
                    ParticleSystem.SubEmittersModule pssem = ps.subEmitters;
                    bool flag3 = pssem.subEmittersCount > 0;
                    if (flag3) goto B1;

                    for (int i = 0; i < pssem.subEmittersCount; ++i)
                    {
                        ParticleSystem psses = pssem.GetSubEmitterSystem(i);
                        bool flag4 = psses.particleCount > 0;
                        if (flag4) goto B1;
                    }
                }
            }

            Util.LogInfo<EffectAutokill>($"Effect {gameObject.name} killed");
            Destroy(this);

        B1:
            return;
        }
    }
}
