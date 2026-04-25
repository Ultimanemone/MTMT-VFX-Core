using MTMTVFX.Core;
using System.Collections;
using UnityEngine;

namespace MTMTVFX.Internal
{
    public class TrailCloneFadeout : EffectManualKill
    {
        private LineRenderer _lr;
        private float _initWidth;
        private float _lifeLeft;

        public override void Init(float lifetime, VFXPool pool)
        {
            _lifetime = lifetime;
            StartCoroutine(ReturnCR(Mathf.Min(lifetime, maxLifetime), pool));


            _lr = GetComponent<LineRenderer>();
            _initWidth = _lr.widthMultiplier;
            _lifeLeft = _lifetime;
        }

        private void Update()
        {
            _lifeLeft = Mathf.Lerp(_lifeLeft, 0, Time.deltaTime * 3f);
            _lr.widthMultiplier = _initWidth * (_lifeLeft / _lifetime);
        }
    }
}
