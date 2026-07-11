using BrilliantSkies.Effects.Pools.Lasers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MTMTVFX.Mono_Scripts
{
    public abstract class PulseBeamUpdater : MonoBehaviour
    {
        public abstract void Fire(Color color, Vector3 start, Vector3 end, float width);
    }
}
