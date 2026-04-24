using MTMTVFX.Core;
using UnityEngine;


namespace MTMTVFX.Internal
{
    public class PooledObj : MonoBehaviour
    {
        private VFXPool _pool;
        protected const float maxLifetime = 12f;

        public virtual void ReturnSelf()
        {
            _pool?.Return(gameObject);
        }

        public void SetPool(VFXPool pool)
        {
            _pool = pool;
        }
    }
}
