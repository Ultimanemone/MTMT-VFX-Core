using MTMTVFX.Core;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Internal
{
    public class EffectAutokill : MonoBehaviour
    {
        public ParticleSystem ps { get; private set; }
        private float maxLifetime = Configurables.maxLifetime;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
        }

        private void LateUpdate()
        {
            bool flag = maxLifetime < ps.time;
            bool flag1 = ps.particleCount < 1;
            if (flag || flag1)
            {
                Destroy(this);
            }
        }
    }
}
