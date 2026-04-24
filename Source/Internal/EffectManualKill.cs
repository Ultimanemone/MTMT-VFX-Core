using MTMTVFX.Core;
using System.Collections;
using UnityEngine;


namespace MTMTVFX.Internal
{
    /// <summary>
    /// Script to kill trail objects
    /// </summary>
    public class EffectManualKill : PooledObj
    {
        public void Init(float lifetime, VFXPool pool)
        {
            StartCoroutine(ReturnCR(Mathf.Min(lifetime, maxLifetime), pool));
        }

        private IEnumerator ReturnCR(float lifetime, VFXPool pool)
        {
            yield return new WaitForSeconds(lifetime);
            ReturnSelf();
            yield break;
        }
    }
}
