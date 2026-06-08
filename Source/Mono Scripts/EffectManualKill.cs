using MTMTVFX.Core.Pooling;
using System.Collections;
using UnityEngine;


namespace MTMTVFX.MonoScripts
{
    /// <summary>
    /// Script to kill trail objects
    /// </summary>
    public class EffectManualKill : PooledObj
    {
        protected float _lifetime;

        public virtual void Init(float lifetime, VFXPool pool)
        {
            _lifetime = lifetime;
            StartCoroutine(ReturnCR(Mathf.Min(lifetime, maxLifetime), pool));
        }

        protected virtual IEnumerator ReturnCR(float lifetime, VFXPool pool)
        {
            yield return new WaitForSeconds(lifetime);
            ReturnSelf();
            yield break;
        }
    }
}
