using MTMTVFX.Core;
using UnityEngine;


namespace MTMTVFX.MonoScripts
{
    /// <summary>
    /// Script to kill vfx objects when they run out of the maximum lifetime, or are no longer rendering particles
    /// </summary>
    public class EffectAutokill : PooledObj
    {
        public ParticleSystem[] psList { get; private set; }

        private void Awake()
        {
            psList = GetComponentsInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            GetComponent<ParticleSystem>().Play(true);
        }

        private void OnParticleSystemStopped()
        {
            Core.Utils.LogInfo<EffectAutokill>($"Effect {gameObject.name} killed");
            ReturnSelf();
        }

        //private void LateUpdate()
        //{
        //    bool flag = (psList[0] == null || maxLifetime < psList[0].time) && psList[0].time > Time.deltaTime;
        //    if (flag)
        //    {
        //        ReturnSelf();
        //    }

        //    foreach (ParticleSystem ps in psList)
        //    {
        //        if (ps.particleCount > 0 || ps.isEmitting) goto B1;
        //    }

        //    Core.Utils.LogInfo<EffectAutokill>($"Effect {gameObject.name} killed");
        //    ReturnSelf();

        //B1:
        //    return;
        //}
    }
}
