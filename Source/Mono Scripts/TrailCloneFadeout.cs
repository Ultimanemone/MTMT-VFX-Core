using MTMTVFX.Core.Pooling;
using System.Collections;
using UnityEngine;

namespace MTMTVFX.MonoScripts
{
    public class TrailCloneFadeout : EffectManualKill
    {
        private LineRenderer _lr;
        private float _initWidth;
        private float _lifeLeft;

        public override void Init(float lifetime, VFXPool pool)
        {
            _lifetime = lifetime;

            _lr = GetComponent<LineRenderer>();
            _initWidth = _lr.widthMultiplier;
            _lifeLeft = _lifetime;
        }

        private void Update()
        {
            if (_lifeLeft <= 0f) ReturnSelf();
            _lifeLeft -= Time.deltaTime;
            _lr.widthMultiplier = _initWidth * (_lifeLeft / _lifetime);
        }
    }
}
