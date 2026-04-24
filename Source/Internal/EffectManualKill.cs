using MTMTVFX.Core;
using System.Collections;
using UnityEngine;


namespace MTMTVFX.Internal
{
    /// <summary>
    /// Script to kill trail objects
    /// </summary>
    public class EffectManualKill : MonoBehaviour
    {
        private const float maxLifetime = 12f;

        public void Init(float lifetime, VFXPool pool)
        {
            StartCoroutine(ReturnCR(Mathf.Min(lifetime, maxLifetime), pool));
        }

        private IEnumerator ReturnCR(float lifetime, VFXPool pool)
        {
            yield return new WaitForSeconds(lifetime);
            pool.Return(gameObject);
            yield break;
        }
    }
}
